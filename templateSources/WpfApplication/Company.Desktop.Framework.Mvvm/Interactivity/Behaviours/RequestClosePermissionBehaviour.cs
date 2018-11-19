using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class RequestClosePermissionBehaviour : InterceptClosingBehaviourBase
	{
		/// <inheritdoc />
		protected override async Task<bool> ShouldCancelAsync()
		{
			var dialogService = Context.ServiceProvider.GetRequiredService<IDialogService>();
			if (await dialogService.ConfirmAsync($"Should the window of type {Context.ViewModel.ToString()} be closed?"))
			{
				return false;
			}

			return true;
		}
	}
}