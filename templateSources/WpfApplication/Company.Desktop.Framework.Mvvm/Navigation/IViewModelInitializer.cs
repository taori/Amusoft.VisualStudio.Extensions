using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface IViewModelInitializer
	{
		Task<bool> ActivateAsync();
	}
}