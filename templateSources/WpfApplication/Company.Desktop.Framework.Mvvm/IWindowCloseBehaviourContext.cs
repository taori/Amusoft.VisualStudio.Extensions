using System;

namespace Company.Desktop.Framework.Mvvm
{
	public interface IWindowCloseBehaviourContext
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}