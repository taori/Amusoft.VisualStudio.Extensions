using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ClosingTracingBehaviour : AsyncBehaviourBase<IWindowClosingBehaviourContext>
	{
		/// <inheritdoc />
		public ClosingTracingBehaviour(string message)
		{
			Message = message;
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ClosingTracingBehaviour));

		public string Message { get; set; }

		/// <inheritdoc />
		protected override Task OnExecuteAsync(IWindowClosingBehaviourContext context)
		{
			if (!string.IsNullOrEmpty(Message))
				Log.Info(Message);
			return Task.CompletedTask;
		}
	}
}