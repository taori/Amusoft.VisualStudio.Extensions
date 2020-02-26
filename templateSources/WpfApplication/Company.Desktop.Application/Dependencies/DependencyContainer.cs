using System;
using Company.Desktop.Application.Dependencies.Setup;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ILogger = NLog.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Company.Desktop.Application.Dependencies
{
	public class DependencyContainer
	{
		private DependencyContainer()
		{
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(DependencyContainer));

		private readonly IServiceCollection _serviceCollection = new ServiceCollection();

		public static readonly DependencyContainer Instance = new DependencyContainer();
		
		public void Configure()
		{
			Log.Debug("Registering manuel services.");
			ManualRegisters(_serviceCollection);
			
			Log.Debug("Discovering registrars.");
			_serviceCollection.DiscoverRegistrars(_serviceCollection.CreateProviderFromFactory(CreateOptions()));

			Log.Debug("Building service provider.");
			var serviceProvider = _serviceCollection.CreateProviderFromFactory(CreateOptions());

			Log.Debug("Creating scoped ServiceProvider");
			var serviceScope = serviceProvider.CreateScope();

			Log.Debug("Assigning service provider.");
			ServiceProvider = serviceScope.ServiceProvider;
		}

		private static ServiceProviderOptions CreateOptions()
		{
			return new ServiceProviderOptions();
		}

		private void ManualRegisters(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();

			serviceCollection.AddSingleton<IInjectionAssemblyLoader, InjectionAssemblyLoader>();
			serviceCollection.AddSingleton<IServiceCollectionFactory>(provider => new ServiceCollectionFactory(serviceCollection));
			serviceCollection.AddSingleton<IObjectComposer, ObjectComposer>();
			serviceCollection.AddSingleton<INestedObjectComposer, NestedObjectComposer>();

			serviceCollection.AddLogging(configure =>
			{
				configure
					.AddNLog()
					.SetMinimumLevel(LogLevel.Trace);
			});
		}

		public IServiceProvider ServiceProvider { get; private set; }


		private class DefaultServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
		{
			/// <inheritdoc />
			public IServiceCollection CreateBuilder(IServiceCollection services)
			{
				return services;
			}

			/// <inheritdoc />
			public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
			{
				var options = new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true };
				return containerBuilder.BuildServiceProvider(options);
			}
		}

		private class ServiceProviderFactory : IServiceProvider
		{
			private IServiceProvider _serviceProvider;

			public ServiceProviderFactory(IServiceProviderFactory<IServiceCollection> factory, IServiceCollection serviceCollection)
			{
				_serviceProvider = factory.CreateServiceProvider(factory.CreateBuilder(serviceCollection));
			}

			/// <inheritdoc />
			public object GetService(Type serviceType)
			{
				return _serviceProvider.GetService(serviceType);
			}
		}
	}
}