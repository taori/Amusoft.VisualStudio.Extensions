using System.Threading.Tasks;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping
{
	[InheritedExport(typeof(IViewModelActivator), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelActivator
	{
		Task<bool> ActivateAsync(IActivateable viewModel);
		Task<bool> ActivateAsync(IActivateable viewModel, object view);
	}
}