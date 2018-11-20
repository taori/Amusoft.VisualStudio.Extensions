using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class CloseTracingBehaviour : IWindowCloseBehaviour
	{
		/// <inheritdoc />
		public CloseTracingBehaviour(string message)
		{
			Message = message;
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(CloseTracingBehaviour));

		public string Message { get; set; }

		/// <inheritdoc />
		public int ExecutionOrder { get; }

		/// <inheritdoc />
		public event EventHandler Executed;

		/// <inheritdoc />
		public IWindowCloseBehaviourContext Context { get; set; }

		/// <inheritdoc />
		public Task ExecuteAsync()
		{
			if (!string.IsNullOrEmpty(Message))
				Log.Info(Message);
			Executed?.Invoke(this, EventArgs.Empty);
			return Task.CompletedTask;
		}
	}
}