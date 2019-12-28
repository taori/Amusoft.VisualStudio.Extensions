using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Extensions
{
	public static class ActivationContextExtensions
	{
		public static Task<bool> NavigateAsync(this IActivationContext context, IWindowViewModel viewModel, string windowId)
		{
			return context.ServiceProvider.GetService<INavigationService>().OpenWindowAsync(viewModel, windowId);
		}
	}
}