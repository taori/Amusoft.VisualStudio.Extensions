using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IViewComposedBehaviourContext : IBehaviourArgument
	{
		IViewCompositionContext CompositionContext { get; }
		IServiceContext ServiceContext { get; }
	}
}