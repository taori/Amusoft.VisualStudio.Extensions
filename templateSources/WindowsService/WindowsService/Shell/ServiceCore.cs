﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace WindowsService.Shell
{
	public class ServiceCore : ServiceBase
	{
		private static readonly ILogger LocalLogger = LogManager.GetLogger(nameof(ServiceCore));

		private static readonly ConcurrentDictionary<JobBase, CancellationTokenSource> CtsDictionary = new ConcurrentDictionary<JobBase, CancellationTokenSource>();

		private static readonly ConcurrentDictionary<JobBase, Task> TaskDictionary = new ConcurrentDictionary<JobBase, Task>();

		public ServiceCore()
		{
			LocalLogger.Trace("Initializing ServiceCore.");
			var serviceCollection = new ServiceCollection();
			LocalLogger.Trace("Building service provider.");
			serviceCollection.BuildServiceProvider();
			var builder = new ServiceBuilder();
			LocalLogger.Trace("Building services.");
			builder.Build(serviceCollection);
			this.ServiceProvider = serviceCollection.BuildServiceProvider(true);
			LocalLogger.Trace("Building composition context.");
			var context = new CompositionContext(ServiceProvider.CreateScope().ServiceProvider);
			LocalLogger.Trace("Composing jobs.");
			this.Jobs = new List<JobBase>(ComposeJobs(context));
			LocalLogger.Info("ServiceCore initialized.");
		}

		public List<JobBase> Jobs { get; }

		public ServiceProvider ServiceProvider { get; }

		public virtual JobComposer CreateJobComposer()
		{
			var composer = new JobComposer();
			return composer;
		}

		private IEnumerable<JobBase> ComposeJobs(CompositionContext context)
		{
			var composer = CreateJobComposer();
			return composer.Compose(context);
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			LocalLogger.Trace($"{nameof(Dispose)}");
			base.Dispose(disposing);
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"Disposing [{job.GetJobName()}].");
					job.Dispose(disposing);
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while disposing [{job.GetJobName()}].");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnContinue()
		{
			LocalLogger.Info($"{nameof(OnContinue)}");
			base.OnContinue();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"Continuing [{job.GetJobName()}].");
					job.OnContinue();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while continuing [{job.GetJobName()}].");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnPause()
		{
			LocalLogger.Info($"{nameof(OnPause)}");
			base.OnPause();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"Pausing [{job.GetJobName()}].");
					job.OnPause();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while pausing [{job.GetJobName()}].");
				}
			}
		}

		/// <inheritdoc />
		protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
		{
			LocalLogger.Info($"{nameof(OnPowerEvent)}");
			var states = new List<bool>();
			states.Add(base.OnPowerEvent(powerStatus));

			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"Power Event [{job.GetJobName()}].");
					states.Add(job.OnPowerEvent(powerStatus));
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while delegating power event [{powerStatus}] [{job.GetJobName()}].");
				}
			}

			return states.All(d => d);
		}

		/// <inheritdoc />
		protected override void OnSessionChange(SessionChangeDescription changeDescription)
		{
			LocalLogger.Trace($"{nameof(OnSessionChange)}");
			base.OnSessionChange(changeDescription);
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"Changing session [{job.GetJobName()}] {changeDescription.Reason}.");
					job.OnSessionChange(changeDescription);
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while changing session [{changeDescription.Reason} : {changeDescription.SessionId}] [{job.GetJobName()}].");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnShutdown()
		{
			LocalLogger.Info($"{nameof(OnShutdown)}");
			base.OnShutdown();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"Shutting down [{job.GetJobName()}].");
					job.OnShutdown();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while shutting down [{job.GetJobName()}].");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnStart(string[] args)
		{
			LocalLogger.Info($"{nameof(OnStart)}");
			base.OnStart(args);
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				CancellationTokenSource cts = null;
				try
				{
					cts = CtsDictionary.GetOrAdd(job, d => new CancellationTokenSource());
					var cancellationToken = cts.Token;
					LocalLogger.Info($"Executing [{job.GetJobName()}].");
					Task.Run(() => job.WorkAsync(args, cancellationToken), cancellationToken);
				}
				catch (TaskCanceledException tce)
				{
					LocalLogger.Info($"Task was cancelled [{job.GetJobName()}].");
					LocalLogger.Debug(tce);
					cts?.Dispose();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while starting [{job.GetJobName()}].");
					cts?.Dispose();
				}
			}
		}

		public async Task ExecuteAsync(string[] args)
		{
			LocalLogger.Info($"{nameof(ExecuteAsync)}");
			var tasks = new List<Task>();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				CancellationTokenSource cts = null;
				try
				{
					cts = CtsDictionary.GetOrAdd(job, d => new CancellationTokenSource());
					var cancellationToken = cts.Token;
					LocalLogger.Info($"Executing [{job.GetJobName()}].");
					tasks.Add(Task.Run(() => job.WorkAsync(args, cancellationToken), cancellationToken));
				}
				catch (TaskCanceledException tce)
				{
					LocalLogger.Info($"Task was cancelled [{job.GetJobName()}].");
					LocalLogger.Debug(tce);
					cts?.Dispose();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while executing [{job.GetJobName()}].");
					cts?.Dispose();
				}
			}

			await Task.WhenAll(tasks);
		}

		/// <inheritdoc />
		protected override void OnStop()
		{
			LocalLogger.Info($"{nameof(OnStop)}");
			base.OnStop();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					if(!CtsDictionary.TryGetValue(job, out var cts))
						LocalLogger.Warn($"No {nameof(CancellationToken)} available.");

					LocalLogger.Info($"Cancelling job [{job.GetJobName()}].");
					cts?.Cancel();

					if(!TaskDictionary.TryGetValue(job, out var task))
						LocalLogger.Warn($"No Task available.");

					LocalLogger.Warn($"Waiting for termination of work for [{job.GetJobName()}].");
					task?.Wait();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while stopping [{job.GetJobName()}].");
				}
			}
		}
	}
}