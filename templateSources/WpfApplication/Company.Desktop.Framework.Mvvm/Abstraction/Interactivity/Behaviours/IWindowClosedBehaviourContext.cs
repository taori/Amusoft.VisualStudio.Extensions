using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IWindowClosedBehaviourContext
	{
		object ViewModel { get; }
		IServiceProvider ServiceProvider { get; }
	}
}