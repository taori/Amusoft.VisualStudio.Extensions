using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IActivationContext
	{
		IServiceProvider ServiceProvider { get; }

		bool Cancelled { get; }

		void Cancel();
	}
}