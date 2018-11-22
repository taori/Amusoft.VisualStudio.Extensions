using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class RequestClosingPermissionBehaviour : InterceptClosingBehaviourBase
	{
		/// <param name="argument"></param>
		/// <inheritdoc />
		protected override async Task<bool> ShouldCancelAsync(IWindowClosingBehaviourContext argument)
		{
			var dialogService = argument.ServiceProvider.GetRequiredService<IDialogService>();
			if (await dialogService.ConfirmAsync($"Should the window of type {argument.ViewModel.ToString()} be closed?"))
			{
				return false;
			}

			return true;
		}
	}
}