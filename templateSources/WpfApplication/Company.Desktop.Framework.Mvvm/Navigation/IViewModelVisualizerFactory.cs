using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.ViewModels;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	[InheritedExport(typeof(IViewModelVisualizerFactory), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelVisualizerFactory
	{
		IViewModelVisualizer Create(IActivateable activateable);
	}
}