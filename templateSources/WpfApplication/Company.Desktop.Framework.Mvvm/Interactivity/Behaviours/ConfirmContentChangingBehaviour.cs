using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ConfirmContentChangingBehaviour : AsyncBehaviourBase<IContentChangingBehaviourContext>
	{
		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IContentChangingBehaviourContext context)
		{
			if (!await context.ServiceProvider.GetRequiredService<IDialogService>().YesNoAsync(context.OldViewModel, "Change content?"))
				context.Cancel();
		}
	}
}