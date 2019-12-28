using System.Threading.Tasks;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ClosingTracingBehavior : AsyncBehaviorBase<IWindowClosingBehaviorContext>
	{
		/// <inheritdoc />
		public ClosingTracingBehavior(string message)
		{
			Message = message;
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ClosingTracingBehavior));

		public string Message { get; set; }

		/// <inheritdoc />
		protected override Task OnExecuteAsync(IWindowClosingBehaviorContext context)
		{
			if (!string.IsNullOrEmpty(Message))
				Log.Info(Message);
			return Task.CompletedTask;
		}
	}
}