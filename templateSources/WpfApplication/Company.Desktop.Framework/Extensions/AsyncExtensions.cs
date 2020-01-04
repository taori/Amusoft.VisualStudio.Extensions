using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Company.Desktop.Framework.Extensions
{
	public static class AsyncExtensions
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(AsyncExtensions));

		public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
		{
			var tcs = new TaskCompletionSource<bool>();
			task.ContinueWith(TaskFaultHandler, TaskContinuationOptions.OnlyOnFaulted);

			using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
			{
				if (task != await Task.WhenAny(task, tcs.Task))
				{
					throw new OperationCanceledException(cancellationToken);
				}
			}

			return task.Result;
		}

		private static void TaskFaultHandler(Task task)
		{
			task.Exception?.Handle(LogMessage);
		}

		private static bool LogMessage(Exception arg)
		{
			Log.Debug(arg);
			return true;
		}
	}
}