using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	[InheritedMefExport(typeof(IViewComposer))]
	public interface IViewComposer
	{
		/// <summary>
		/// Order in which the Composer is prioritized for a specific dataContext
		/// </summary>
		int Priority { get; }

		Task<bool> ComposeAsync(IViewCompositionContext context);

		bool CanHandle(FrameworkElement control);
	}
}