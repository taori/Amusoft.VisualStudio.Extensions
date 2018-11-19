using System.Threading.Tasks;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Navigation;

namespace Company.Desktop.Framework.Mvvm
{
	[InheritedExport(typeof(IViewModelActivator), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelActivator
	{
		Task<bool> ActivateAsync(IActivateable viewModel);
		Task<bool> ActivateAsync(IActivateable viewModel, object view);
	}
}