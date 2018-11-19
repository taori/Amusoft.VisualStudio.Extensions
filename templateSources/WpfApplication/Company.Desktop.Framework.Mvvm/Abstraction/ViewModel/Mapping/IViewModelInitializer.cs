using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping
{
	public interface IViewModelInitializer
	{
		Task<bool> ActivateAsync();
	}
}