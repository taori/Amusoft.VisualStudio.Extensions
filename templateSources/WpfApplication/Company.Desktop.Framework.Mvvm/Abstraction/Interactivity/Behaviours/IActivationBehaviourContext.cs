using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IActivationBehaviourContext : IBehaviourArgument
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}