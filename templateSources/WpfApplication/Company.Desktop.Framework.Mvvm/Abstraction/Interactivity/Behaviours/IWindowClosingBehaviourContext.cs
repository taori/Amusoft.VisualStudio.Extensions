using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IWindowClosingBehaviourContext : IBehaviourArgument
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}