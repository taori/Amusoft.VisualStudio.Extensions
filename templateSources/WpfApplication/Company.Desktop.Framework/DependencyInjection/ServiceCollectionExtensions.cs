using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ServiceCollectionExtensions));

		public static IServiceCollection RegisterFrom<T>(this IServiceCollection services, T bootstrapper)
			where T : IServiceRegistrar
		{
			if(bootstrapper == null)
				throw new ArgumentNullException(nameof(bootstrapper));

			Log.Debug($"Registering from bootstrapper {bootstrapper.GetType().FullName}");
			bootstrapper.Register(services);
			return services;
		}

		public static IServiceCollection DiscoverRegistrars(this IServiceCollection services, IServiceProvider serviceProvider)
		{
			var registrarLoader = new RegistrarLoader(serviceProvider.GetRequiredService<IInjectionAssemblyLoader>());
			registrarLoader.Register(services);
			return services;
		}
	}
}