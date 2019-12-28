using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public interface IRegionManager
	{
		FrameworkElement GetControl(object regionViewModelHolder, string regionName);
		FrameworkElement GetHostingWindow(object viewModel);
	}
}