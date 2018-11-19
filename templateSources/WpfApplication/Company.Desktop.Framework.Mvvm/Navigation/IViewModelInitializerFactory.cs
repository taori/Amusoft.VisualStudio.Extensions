using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	[InheritedExport(typeof(IViewModelInitializerFactory), LifeTime = LifeTime.Singleton)]
	public interface IViewModelInitializerFactory
	{
		IViewModelInitializer Create(IActivateable activateable);

		IViewModelInitializer Create(IActivateable activateable, object view);

		bool CanHandle(IActivateable activateable);
	}
}