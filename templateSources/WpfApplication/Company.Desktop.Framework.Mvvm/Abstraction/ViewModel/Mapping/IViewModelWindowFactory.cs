using System.Windows;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping
{
	[InheritedExport(typeof(IViewModelWindowFactory), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelWindowFactory
	{
		bool CanCreateWindow(IActivateable activateable);

		Window CreateWindow(IActivateable activateable);
	}
}