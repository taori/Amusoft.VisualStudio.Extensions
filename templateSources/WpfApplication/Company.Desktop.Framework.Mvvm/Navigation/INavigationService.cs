using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface INavigationService
	{
		Task<bool> OpenWindowAsync(IWindowViewModel viewModel, string windowId);
	}
}