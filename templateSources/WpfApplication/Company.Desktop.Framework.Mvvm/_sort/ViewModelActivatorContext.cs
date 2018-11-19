using System;

namespace Company.Desktop.Framework.Mvvm._sort
{
	public interface IViewModelActivatorContext : IServiceContext
	{
	}

	public interface IServiceContext
	{
		IServiceProvider ServiceProvider { get; }
	}

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

	public class ViewModelActivatorContext : ServiceContext, IViewModelActivatorContext
	{
		/// <inheritdoc />
		public ViewModelActivatorContext(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}
	}
}