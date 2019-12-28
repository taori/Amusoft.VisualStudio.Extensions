using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ConfirmContentChangingBehavior : AsyncBehaviorBase<IContentChangingBehaviorContext>
	{
		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IContentChangingBehaviorContext context)
		{
			if (!await context.ServiceProvider.GetRequiredService<IDialogService>().YesNoAsync(context.OldViewModel, "Change content?"))
				context.Cancel();
		}
	}
}