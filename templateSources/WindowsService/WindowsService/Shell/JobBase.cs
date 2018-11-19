using System.Linq.Expressions;
using System.ServiceProcess;
using System.Threading.Tasks;
using NLog;

namespace WindowsService.Shell
{
	public abstract class JobBase
	{
		private ILogger _logger;

		protected ILogger Logger => _logger ?? (_logger = CreateLogger());

		private ILogger CreateLogger()
		{
			return LogManager.GetLogger(GetType().FullName);
		}

		/// <summary>
		/// Lower order wins
		/// </summary>
		public virtual int Priority { get; } = 0;

		public virtual void OnContinue() => Expression.Empty();

		public virtual void OnPause() => Expression.Empty();

		public virtual bool OnPowerEvent(PowerBroadcastStatus powerStatus)
		{
			return true;
		}

		public virtual void OnSessionChange(SessionChangeDescription changeDescription) => Expression.Empty();

		public virtual string GetJobName() => GetType().Name;

		public abstract void OnShutdown();

		public abstract void OnStop();

		public abstract void Dispose(bool disposing);

		public abstract Task WorkAsync(string[] args);
	}
}