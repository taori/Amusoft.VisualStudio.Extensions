using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsService.Shell;

namespace WindowsService.Jobs
{
	public interface ITest
	{
		void Test();
	}

	public class TestImpl : ITest
	{
		/// <inheritdoc />
		public void Test()
		{
		}
	}

	public class SampleJob : JobBase
	{
		public ITest Test { get; }

		public SampleJob(ITest test)
		{
			Test = test;
		}

		/// <inheritdoc />
		public override void OnShutdown()
		{
		}

		/// <inheritdoc />
		public override void Dispose(bool disposing)
		{
		}

		/// <inheritdoc />
		public override async Task WorkAsync(string[] args, CancellationToken cancellationToken)
		{
			try
			{
				while (true)
				{
					Logger.Warn("(Server) still running.");
					cancellationToken.ThrowIfCancellationRequested();
					await Task.Delay(60000, cancellationToken);
				}
			}
			catch (Exception e)
			{
				Logger.Fatal("Server crashed.");
				Logger.Fatal(e);
			}

			Logger.Info("Application terminated.");
		}
	}
}