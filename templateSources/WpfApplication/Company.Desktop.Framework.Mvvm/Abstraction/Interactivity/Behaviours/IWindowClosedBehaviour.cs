using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IWindowClosedBehaviour : IAsyncBehaviour<IWindowClosedBehaviourContext>
	{
	}

	public interface IWindowClosedBehaviourContext : IBehaviourArgument
	{
		object ViewModel { get; }
		IServiceProvider ServiceProvider { get; }
	}
}