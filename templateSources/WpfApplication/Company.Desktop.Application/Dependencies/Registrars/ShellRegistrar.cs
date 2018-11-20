using Company.Desktop.Application.Dependencies.Setup;
using Company.Desktop.Application.Dependencies.UI;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Integration;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Application.Dependencies.Registrars
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
			services.AddSingleton<IWindowManager, WindowManager>();
			services.AddSingleton<IDisplayCoordinatorFactory, DisplayCoordinatorFactory>();

			services.AddTransient<IServiceContext, ServiceContext>();
			services.AddTransient<IViewModelWindowFactory, WindowFactory>();
		}
	}
}