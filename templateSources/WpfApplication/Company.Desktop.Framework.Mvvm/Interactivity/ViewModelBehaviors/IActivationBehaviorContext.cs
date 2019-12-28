using System;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public interface IActivationBehaviorContext : IBehaviorArgument
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}