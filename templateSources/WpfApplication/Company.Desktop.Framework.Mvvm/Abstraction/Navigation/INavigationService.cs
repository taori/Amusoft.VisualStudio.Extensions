using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Navigation
{
	public interface INavigationService
	{
		Task<bool> OpenWindowAsync(IWindowViewModel viewModel, string windowId);
	}
}