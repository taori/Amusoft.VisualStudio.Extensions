using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm
{
	public static class ActivationContextExtensions
	{
		public static Task<bool> NavigateAsync(this IActivationContext context, IWindowViewModel viewModel)
		{
			return context.ServiceProvider.GetService<INavigationService>().OpenWindowAsync(viewModel);
		}
	}
}