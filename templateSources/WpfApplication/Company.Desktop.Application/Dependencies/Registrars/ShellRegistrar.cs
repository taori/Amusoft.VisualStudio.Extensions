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
using NLog;

namespace Company.Desktop.Application.Dependencies.Registrars
{
	public class ShellRegistrar : IServiceRegistrar
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ShellRegistrar));

		/// <inheritdoc />
		public void Register(IServiceCollection services)
		{
			Singleton<IDialogService, MetroDialogService>(services);
			Singleton<INavigationService, NavigationService>(services);
			Singleton<IInjectionAssemblyLoader, InjectionAssemblyLoader>(services);
			Singleton<IRegionManager, RegionManager>(services);
			Singleton<IWindowManager, WindowManager>(services);
			Singleton<IDisplayCoordinatorFactory, DisplayCoordinatorFactory>(services);
			Singleton<IBehaviourRunner, BehaviourRunner>(services);
			Singleton<ISettingsStorage, SettingsStorage>(services);

			Transient<IServiceContext, ServiceContext>(services);
			Transient<IViewModelWindowFactory, WindowFactory>(services);
			services.AddTransient<IRegexDataTemplatePatternProvider>(CreateDefaultConventionPattern);
		}

		private void Singleton<TService, TImplementation>(IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			Log.Debug($"Registering [Singleton] [{typeof(TImplementation)}] -> [{typeof(TService)}].");
			services.AddSingleton<TService, TImplementation>();
		}
		private void Transient<TService, TImplementation>(IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			Log.Debug($"Registering [Transient] [{typeof(TImplementation)}] -> [{typeof(TService)}].");
			services.AddTransient<TService, TImplementation>();
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