using System.Windows;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Navigation;

namespace Company.Desktop.Framework.Mvvm
{
	[InheritedExport(typeof(IViewModelWindowFactory), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelWindowFactory
	{
		bool CanCreateWindow(IActivateable activateable);

		Window CreateWindow(IActivateable activateable);
	}
}