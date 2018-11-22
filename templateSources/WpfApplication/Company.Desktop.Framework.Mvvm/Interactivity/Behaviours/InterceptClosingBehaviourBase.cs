using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class InterceptClosingBehaviourBase : AsyncBehaviourBase<IWindowClosingBehaviourContext>
	{
		protected abstract Task<bool> ShouldCancelAsync(IWindowClosingBehaviourContext argument);

		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IWindowClosingBehaviourContext context)
		{
			if (await ShouldCancelAsync(context))
				context.Cancel();
		}
	}
}