using System.Windows;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewComposerFactory))]
	public interface IViewComposerFactory
	{
		IViewComposer Create(FrameworkElement control);
	}
}