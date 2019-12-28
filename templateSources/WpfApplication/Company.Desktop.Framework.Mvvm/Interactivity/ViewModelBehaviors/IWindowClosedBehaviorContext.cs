using System;
using Company.Desktop.Framework.Mvvm.Integration.Composer;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public interface IWindowClosedBehaviorContext : IBehaviorArgument
	{
		object ViewModel { get; }

		IServiceProvider ServiceProvider { get; }

		IViewCompositionContext Context { get; }
	}
}