using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Abstraction.UI
{
	public interface IRegionManager
	{
		FrameworkElement GetControl(object regionViewModelHolder, string regionName);
	}
}