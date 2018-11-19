using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.ViewModels;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface INavigationService
	{
		Task<bool> OpenWindowAsync(IWindowViewModel viewModel);
	}
}