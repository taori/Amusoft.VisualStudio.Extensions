using System;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm
{
	[InheritedExport(typeof(ViewModelActivatorContext), LifeTime = LifeTime.PerRequest)]
	public class ViewModelActivatorContext
	{
		public IServiceProvider ServiceProvider { get; }

		public ViewModelActivatorContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}
	}
}