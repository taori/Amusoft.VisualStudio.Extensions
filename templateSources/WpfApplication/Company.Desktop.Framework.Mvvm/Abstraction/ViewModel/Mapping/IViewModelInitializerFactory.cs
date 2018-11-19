using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping
{
	[InheritedExport(typeof(IViewModelInitializerFactory), LifeTime = LifeTime.Singleton)]
	public interface IViewModelInitializerFactory
	{
		IViewModelInitializer Create(IActivateable activateable);

		IViewModelInitializer Create(IActivateable activateable, object view);

		bool CanHandle(IActivateable activateable);
	}
}