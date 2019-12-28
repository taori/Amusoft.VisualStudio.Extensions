using Company.Desktop.Framework.Mvvm.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Integration.Environment;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public interface IViewComposedBehaviorContext : IBehaviorArgument
	{
		IViewCompositionContext CompositionContext { get; }
		IServiceContext ServiceContext { get; }
	}
}