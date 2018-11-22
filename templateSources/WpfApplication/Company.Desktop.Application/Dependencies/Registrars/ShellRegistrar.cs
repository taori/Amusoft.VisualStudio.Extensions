using System;
using System.Text.RegularExpressions;
using Company.Desktop.Application.Dependencies.Setup;
using Company.Desktop.Application.Dependencies.UI;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
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
			services.AddSingleton<IBehaviourRunner, BehaviourRunner>();

			services.AddTransient<IServiceContext, ServiceContext>();
			services.AddTransient<IViewModelWindowFactory, WindowFactory>();
			services.AddTransient<IRegexDataTemplatePatternProvider>(CreateDefaultConventionPattern);
		}

		private static InlineMvvmPattern CreateDefaultConventionPattern(IServiceProvider provider)
		{
			// TODO: change the contents of this regex to match your namespace
			return new InlineMvvmPattern(
				new Regex("(?<ns1>Company\\.Desktop\\.)(?<ignore>ViewModels\\.)(?<ns2>.+)(?<class>\\.[^.]+)(?=ViewModel)", RegexOptions.Compiled, TimeSpan.FromMilliseconds(30)), 
				new Regex("(?<ns1>Company\\.Desktop\\.)(?<ignore>Application\\.Views\\.)(?<ns2>.+)(?<class>\\.[^.]+)(?=View|Page|Control|Window)", RegexOptions.Compiled, TimeSpan.FromMilliseconds(30))
			);
		}
	}
}