using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IViewComposedBehaviorContext : IBehaviorArgument
	{
		IViewCompositionContext CompositionContext { get; }
		IServiceContext ServiceContext { get; }
	}
}