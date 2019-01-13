using System.Windows;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewComposerHook))]
	public interface IViewComposerHook
	{
		void Execute(FrameworkElement control, object dataContext);
	}
}