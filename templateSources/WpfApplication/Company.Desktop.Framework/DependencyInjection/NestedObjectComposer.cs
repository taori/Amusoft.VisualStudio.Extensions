using System;
using Company.Desktop.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Desktop.Framework.DependencyInjection
{
	public class NestedObjectComposer : INestedObjectComposer
	{
		private readonly IServiceCollectionFactory _serviceCollectionFactory;

		public NestedObjectComposer(IServiceCollectionFactory serviceCollectionFactory)
		{
			_serviceCollectionFactory = serviceCollectionFactory;
		}

		/// <inheritdoc />
		public T Compose<T>(Action<IServiceCollection> beforeCompose = null) where T : class
		{
			var serviceCollection = _serviceCollectionFactory.FromOrigin();
			beforeCompose?.Invoke(serviceCollection);

			serviceCollection.Replace(ServiceDescriptor.Describe(typeof(IServiceCollectionFactory), provider => new ServiceCollectionFactory(serviceCollection), ServiceLifetime.Singleton));
			var composer = serviceCollection.CreateProviderFromFactory().GetRequiredService<IObjectComposer>();

			return composer.Compose<T>();
		}
	}
}