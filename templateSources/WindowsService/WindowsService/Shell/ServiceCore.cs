using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace WindowsService.Shell
{
	public class ServiceCore : ServiceBase
	{
		private static readonly ILogger LocalLogger = LogManager.GetLogger(nameof(ServiceCore));

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
			LocalLogger.Trace("Building composition context");
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
			LocalLogger.Trace($"{nameof(ServiceCore)} - {nameof(Dispose)}");
			base.Dispose(disposing);
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Disposing {job.GetJobName()}");
					job.Dispose(disposing);
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while disposing {job.GetJobName()}");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnContinue()
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(OnContinue)}");
			base.OnContinue();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Continuing {job.GetJobName()}");
					job.OnContinue();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while continuing {job.GetJobName()}");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnPause()
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(OnPause)}");
			base.OnPause();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Pausing {job.GetJobName()}");
					job.OnPause();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while pausing {job.GetJobName()}");
				}
			}
		}

		/// <inheritdoc />
		protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(OnPowerEvent)}");
			var states = new List<bool>();
			states.Add(base.OnPowerEvent(powerStatus));

			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Power Event {job.GetJobName()}");
					states.Add(job.OnPowerEvent(powerStatus));
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while delegating power event [{powerStatus}] {job.GetJobName()}");
				}
			}

			return states.All(d => d);
		}

		/// <inheritdoc />
		protected override void OnSessionChange(SessionChangeDescription changeDescription)
		{
			LocalLogger.Trace($"{nameof(ServiceCore)} - {nameof(OnSessionChange)}");
			base.OnSessionChange(changeDescription);
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Changing session {job.GetJobName()}");
					job.OnSessionChange(changeDescription);
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while changing session [{changeDescription.Reason} : {changeDescription.SessionId}] {job.GetJobName()}");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnShutdown()
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(OnShutdown)}");
			base.OnShutdown();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Shutting down {job.GetJobName()}");
					job.OnShutdown();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while shutting down {job.GetJobName()}");
				}
			}
		}

		/// <inheritdoc />
		protected override void OnStart(string[] args)
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(OnStart)}");
			base.OnStart(args);
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Executing {job.GetJobName()}");
					Task.Run(() => job.WorkAsync(args));
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while starting {job.GetJobName()}");
				}
			}
		}

		public Task ExecuteAsync(string[] args)
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(ExecuteAsync)}");
			var tasks = new List<Task>();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Executing {job.GetJobName()}");
					tasks.Add(Task.Run(() => job.WorkAsync(args)));
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while executing {job.GetJobName()}");
				}
			}

			return Task.WhenAll(tasks);
		}

		/// <inheritdoc />
		protected override void OnStop()
		{
			LocalLogger.Info($"{nameof(ServiceCore)} - {nameof(OnStop)}");
			base.OnStop();
			foreach (var job in Jobs.OrderByDescending(d => d.Priority))
			{
				try
				{
					LocalLogger.Info($"{nameof(ServiceCore)} - Stopping {job.GetJobName()}");
					job.OnStop();
				}
				catch (Exception e)
				{
					LocalLogger.Fatal(e, $"An error occured while stopping {job.GetJobName()}");
				}
			}
		}
	}
}