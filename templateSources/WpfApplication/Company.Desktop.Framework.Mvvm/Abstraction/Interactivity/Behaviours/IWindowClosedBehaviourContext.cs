using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IWindowClosedBehaviourContext : IBehaviourArgument
	{
		object ViewModel { get; }

		IServiceProvider ServiceProvider { get; }

		IViewCompositionContext Context { get; }
	}
}