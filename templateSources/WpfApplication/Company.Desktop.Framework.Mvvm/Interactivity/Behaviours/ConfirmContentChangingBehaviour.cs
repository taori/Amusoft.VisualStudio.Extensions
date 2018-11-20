using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ConfirmContentChangingBehaviour : ContentChangingBehaviourBase
	{
		/// <inheritdoc />
		protected override async Task OnExecuteAsync()
		{
			if(!await Context.ServiceProvider.GetRequiredService<IDialogService>().ConfirmAsync("Change content?"))
				Context.Cancel();
		}
	}
}