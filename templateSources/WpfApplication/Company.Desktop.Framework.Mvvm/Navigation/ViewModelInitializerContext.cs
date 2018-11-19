using System;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	[InheritedExport(typeof(ViewModelInitializerContext), LifeTime = LifeTime.PerRequest)]
	public class ViewModelInitializerContext
	{
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public ViewModelInitializerContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}
	}
}