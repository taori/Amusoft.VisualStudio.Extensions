using System.Threading.Tasks;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	[InheritedExport(typeof(IDisplayCoordinator), LifeTime = LifeTime.PerRequest)]
	public interface IDisplayCoordinator
	{
		/// <summary>
		/// Order in which the VisualizerFactory will be attempt to construct a Visualizer
		/// </summary>
		int FactoryOrder { get; }

		bool CanProcess(object dataContext);

		Task<bool> DisplayAsync(object dataContext, ICoordinationArguments coordinationArguments);
	}
}