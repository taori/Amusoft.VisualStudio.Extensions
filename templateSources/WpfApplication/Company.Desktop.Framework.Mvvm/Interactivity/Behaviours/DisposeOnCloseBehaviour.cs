using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class DisposeOnCloseBehaviour : AsyncBehaviourBase<IWindowClosedBehaviourContext>
	{
		/// <inheritdoc />
		protected override Task OnExecuteAsync(IWindowClosedBehaviourContext context)
		{
			if (context.ViewModel is IDisposable disposable)
				disposable.Dispose();

			return Task.CompletedTask;
		}
	}
}