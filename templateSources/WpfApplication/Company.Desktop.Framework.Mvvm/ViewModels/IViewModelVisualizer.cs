using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Navigation;

namespace Company.Desktop.Framework.Mvvm.ViewModels
{
	[InheritedExport(typeof(IViewModelVisualizer), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelVisualizer
	{
		Task<bool> VisualizeAsync(IActivateable activateable, FrameworkElement element);
	}
}