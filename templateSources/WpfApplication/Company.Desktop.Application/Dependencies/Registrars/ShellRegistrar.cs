using Company.Desktop.Application.Dependencies.Setup;
using Company.Desktop.Application.Dependencies.UI;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm._sort;
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
			services.AddSingleton<IViewModelVisualizerFactory, ViewModelVisualizerFactory>();

			services.AddTransient<IServiceContext, ServiceContext>();
			services.AddTransient<IViewModelActivatorContext, ViewModelActivatorContext>();
		}
	}
}