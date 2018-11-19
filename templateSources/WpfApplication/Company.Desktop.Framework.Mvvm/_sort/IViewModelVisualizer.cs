using System.Threading.Tasks;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm._sort
{
	[InheritedExport(typeof(IViewModelVisualizer), LifeTime = LifeTime.PerRequest)]
	public interface IViewModelVisualizer
	{
		/// <summary>
		/// Order in which the VisualizerFactory will be attempt to construct a Visualizer
		/// </summary>
		int FactoryOrder { get; }

		bool CanProcess(IActivateable activateable);

		Task<bool> VisualizeAsync(IActivateable activateable, ICoordinationArguments coordinationArguments);
	}
}