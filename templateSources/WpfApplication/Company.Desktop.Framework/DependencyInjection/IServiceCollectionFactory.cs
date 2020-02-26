using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public interface IServiceCollectionFactory
	{
		IServiceCollection FromOrigin();
	}
}