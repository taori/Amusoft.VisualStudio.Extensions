
using System.Threading.Tasks;
using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface IRegionManager
	{
		FrameworkElement GetControl(object regionViewModelHolder, string regionName);
	}
}