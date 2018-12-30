using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IWindowClosingBehaviorContext : IBehaviorArgument
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}