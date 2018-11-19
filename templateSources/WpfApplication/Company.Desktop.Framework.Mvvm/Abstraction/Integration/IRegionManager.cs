using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration
{
	public interface IRegionManager
	{
		FrameworkElement GetControl(object regionViewModelHolder, string regionName);
	}
}