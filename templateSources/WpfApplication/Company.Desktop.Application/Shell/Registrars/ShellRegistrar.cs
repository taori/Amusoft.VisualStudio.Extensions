using Company.Desktop.Application.Shell.Dependencies;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Application.Shell.Registrars
{
	public class ShellRegistrar : IServiceRegistrar
	{
		/// <inheritdoc />
		public void Register(IServiceCollection services)
		{
			services.AddSingleton<IDialogService, DialogService>();
			services.AddSingleton<INavigationService, NavigationService>();
			services.AddSingleton<IInjectionAssemblyLoader, InjectionAssemblyLoader>();
			services.AddSingleton<IRegionManager, RegionManager>();
		}
	}
}