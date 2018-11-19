
using System.Composition.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public static class ContainerConfigurationExtensions
	{
		public static ContainerConfiguration WithServiceCollection(this ContainerConfiguration source, IServiceCollection collection)
		{
			source.WithProvider(new CustomExportProvider(collection));
			return source;
		}
	}
}