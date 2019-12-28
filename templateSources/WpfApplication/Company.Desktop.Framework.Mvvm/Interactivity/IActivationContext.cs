using System;

namespace Company.Desktop.Framework.Mvvm.Interactivity
{
	public interface IActivationContext
	{
		IServiceProvider ServiceProvider { get; }

		bool Cancelled { get; }

		void Cancel();
	}
}