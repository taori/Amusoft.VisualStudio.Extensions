using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public interface IServiceRegistrar
	{
		void Register(IServiceCollection services);
	}
}