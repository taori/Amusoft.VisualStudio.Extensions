using WindowsService.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace WindowsService
{
	public class ServiceBuilder
	{
		public void Build(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<ITest>(sp => new TestImpl());
		}
	}
}