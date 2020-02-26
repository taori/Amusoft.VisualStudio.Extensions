using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Desktop.Framework.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceProvider CreateProviderFromFactory(this IServiceCollection source, ServiceProviderOptions options = null)
		{
			options = options ?? new ServiceProviderOptions();

			// see https://github.com/dotnet/aspnetcore/blob/bc6fb44840e10343548b4a0178f0ce7653a5222a/src/Hosting/Hosting/src/WebHostBuilder.cs#L199
			using (var provider = source.BuildServiceProvider(options))
			{
				var factory = provider.GetRequiredService<IServiceProviderFactory<IServiceCollection>>();
				return factory.CreateServiceProvider(factory.CreateBuilder(source));
			}
		}

		public static IServiceCollection Clone(this IServiceCollection source)
		{
			var collection = new ServiceCollection();
			ServiceDescriptor[] array = new ServiceDescriptor[source.Count];
			source.CopyTo(array, 0);
			foreach (var descriptor in array)
			{
				collection.Add(descriptor);
			}

			return collection;
		}
	}
}