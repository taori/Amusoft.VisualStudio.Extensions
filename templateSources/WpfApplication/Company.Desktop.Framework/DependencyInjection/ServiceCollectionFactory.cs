using Company.Desktop.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public class ServiceCollectionFactory : IServiceCollectionFactory
	{
		private readonly IServiceCollection _source;

		public ServiceCollectionFactory(IServiceCollection source)
		{
			_source = source;
		}

		/// <inheritdoc />
		public IServiceCollection FromOrigin()
		{
			return _source.Clone();
		}
	}
}