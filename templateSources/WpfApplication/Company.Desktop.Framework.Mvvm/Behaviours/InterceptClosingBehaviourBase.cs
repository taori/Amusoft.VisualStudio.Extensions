using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Extensibility;

namespace Company.Desktop.Framework.Mvvm.Behaviours
{
	public abstract class InterceptClosingBehaviourBase : IWindowCloseBehaviour
	{
		protected abstract Task<bool> ShouldCancelAsync();

		/// <inheritdoc />
		public int ExecutionOrder { get; }

		/// <inheritdoc />
		public event EventHandler Executed;

		/// <inheritdoc />
		public IWindowCloseBehaviourContext Context { get; set; }

		/// <inheritdoc />
		public async Task ExecuteAsync()
		{
			if (await ShouldCancelAsync())
				Context.Cancel();

			Executed?.Invoke(this, EventArgs.Empty);
		}
	}
}