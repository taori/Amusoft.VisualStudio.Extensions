using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IWindowCloseBehaviourContext
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}