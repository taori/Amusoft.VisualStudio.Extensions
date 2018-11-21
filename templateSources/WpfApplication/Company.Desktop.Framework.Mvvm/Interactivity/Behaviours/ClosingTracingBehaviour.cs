using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ClosingTracingBehaviour : BehaviourBase, IWindowClosingBehaviour
	{
		/// <inheritdoc />
		public ClosingTracingBehaviour(string message)
		{
			Message = message;
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ClosingTracingBehaviour));

		public string Message { get; set; }

		/// <inheritdoc />
		public IWindowClosingBehaviourContext Context { get; private set; }

		/// <inheritdoc />
		public Task ExecuteAsync()
		{
			if (!string.IsNullOrEmpty(Message))
				Log.Info(Message);
			RaiseExecuted();
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public void SetContext(IWindowClosingBehaviourContext context)
		{
			Context = context;
		}
	}
}