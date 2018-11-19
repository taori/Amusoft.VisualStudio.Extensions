using System;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface IActivationContext
	{
		IServiceProvider ServiceProvider { get; }

		bool Cancelled { get; }

		void Cancel();
	}
}