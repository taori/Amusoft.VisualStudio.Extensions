using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm._sort;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class FrameworkElementVisualizer : IViewModelVisualizer
	{
		public IRegionManager RegionManager { get; }
		public IViewModelActivator Activator { get; }

		public FrameworkElementVisualizer(IRegionManager regionManager, IViewModelActivator activator)
		{
			RegionManager = regionManager;
			Activator = activator;
		}

		/// <inheritdoc />
		public int FactoryOrder { get; }

		/// <inheritdoc />
		public bool CanProcess(IActivateable activateable)
		{
			return activateable is IContentViewModel;
		}

		/// <inheritdoc />
		public async Task<bool> VisualizeAsync(IActivateable activateable, ICoordinationArguments coordinationArguments)
		{
			if (coordinationArguments is RegionArguments arguments)
			{
				var control = RegionManager.GetControl(arguments.RegionManagerReference, arguments.TargetRegion);
				if (activateable is IBusyStateHolder busyStateHolder)
				{
					using(busyStateHolder.LoadingState.Session())
					{
						return await Activator.ActivateAsync(activateable, control);
					}
				}
				else
				{
					return await Activator.ActivateAsync(activateable, control);
				}
			}

			return false;
		}
	}
}