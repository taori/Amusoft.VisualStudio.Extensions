
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewContextBinder))]
	public interface IViewContextBinder
	{
		bool TryBind(IViewCompositionContext context);
	}
}