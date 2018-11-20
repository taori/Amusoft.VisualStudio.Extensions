
using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class ContentChangingBehaviourBase : IContentChangingBehaviour
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ContentChangingBehaviourBase));

		/// <inheritdoc />
		public int ExecutionOrder { get; }

		/// <inheritdoc />
		public event EventHandler Executed;

		/// <inheritdoc />
		public IContentChangingBehaviourContext Context { get; set; }

		/// <inheritdoc />
		public async Task ExecuteAsync()
		{
			try
			{
				await OnExecuteAsync();
				Executed?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception e)
			{
				Log.Error(e);
				Context.Cancel();
			}
			finally
			{
				Executed?.Invoke(this, EventArgs.Empty);
			}
		}

		protected abstract Task OnExecuteAsync();
	}
}