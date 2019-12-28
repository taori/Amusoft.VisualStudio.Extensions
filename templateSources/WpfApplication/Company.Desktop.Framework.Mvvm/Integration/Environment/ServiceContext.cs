using System;

namespace Company.Desktop.Framework.Mvvm.Integration.Environment
{
	public class ServiceContext : IServiceContext
	{
		/// <inheritdoc />
		public ServiceContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }
	}
}