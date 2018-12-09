using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.DependencyInjection
{
	internal class CustomExportProvider : ExportDescriptorProvider
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(CustomExportProvider));

		public IServiceCollection Services { get; }

		public IServiceProvider ServiceProvider { get; }

		public ILookup<Type, ServiceDescriptor> ServicesByType { get; }

		public CustomExportProvider(IServiceCollection services)
		{
			Services = services;
			ServicesByType = Services.ToLookup(d => d.ServiceType);
			var sp = new DefaultServiceProviderFactory(new ServiceProviderOptions() { ValidateScopes = true });
			Log.Debug("Creating ServiceScope for type resolution.");
			ServiceProvider = sp.CreateServiceProvider(services).CreateScope().ServiceProvider;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(
			CompositionContract contract,
			DependencyAccessor descriptorAccessor)
		{
			var result = new List<ExportDescriptorPromise>();
			foreach (var service in ServicesByType[contract.ContractType])
			{
				Log.Debug($"Providing {service.ImplementationType.FullName} as {contract.ContractType.FullName}.");
				var promise = new ExportDescriptorPromise(contract, nameof(CustomExportProvider), true, () => GetDependencies(service, contract, descriptorAccessor),
					dependencies => ExportDescriptor.Create(
						activator: (context, o) => ServiceProvider.GetRequiredService(contract.ContractType), 
						metadata: NoMetadata));
				result.Add(promise);
			}

			return result;
		}

		private IEnumerable<CompositionDependency> GetDependencies(ServiceDescriptor service, CompositionContract contract, DependencyAccessor descriptorAccessor)
		{
			var constructorWithArgs = service.ImplementationType.GetConstructors().FirstOrDefault(d => d.GetParameters().Length > 0);
			if (constructorWithArgs == null)
				yield break;

			Log.Warn($"A constructor with parameters is used. This area is not implemented properly yet.");
			foreach (var parameter in constructorWithArgs.GetParameters())
			{
				// this code is untested. not sure how to implement it.
				yield return CompositionDependency.Missing(contract, parameter.ParameterType);
			}
		}
	}
}