using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedExport(typeof(IViewComposer))]
	public interface IViewComposer
	{
		/// <summary>
		/// Order in which the Composer is prioritized for a specific dataContext
		/// </summary>
		int FactoryPriority { get; }

		Task<bool> ComposeAsync(IViewCompositionContext context);

		bool CanHandle(FrameworkElement control);
	}
}