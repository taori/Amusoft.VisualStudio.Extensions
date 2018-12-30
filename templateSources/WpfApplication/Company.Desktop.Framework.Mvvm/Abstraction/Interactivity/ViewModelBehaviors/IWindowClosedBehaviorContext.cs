using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IWindowClosedBehaviorContext : IBehaviorArgument
	{
		object ViewModel { get; }

		IServiceProvider ServiceProvider { get; }

		IViewCompositionContext Context { get; }
	}
}