using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm._sort;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping
{
	public interface IViewModelVisualizerFactory
	{
		IViewModelVisualizer Create(IActivateable activateable);
	}
}