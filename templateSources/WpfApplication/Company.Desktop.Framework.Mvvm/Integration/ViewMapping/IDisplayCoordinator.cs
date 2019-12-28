using System.Threading.Tasks;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	[InheritedMefExport(typeof(IDisplayCoordinator), LifeTime = LifeTime.PerRequest)]
	public interface IDisplayCoordinator
	{
		/// <summary>
		/// Order in which the VisualizerFactory will be attempt to construct a Visualizer
		/// </summary>
		int Priority { get; }

		bool CanProcess(object dataContext);

		Task<bool> DisplayAsync(object dataContext, ICoordinationArguments coordinationArguments);
	}
}