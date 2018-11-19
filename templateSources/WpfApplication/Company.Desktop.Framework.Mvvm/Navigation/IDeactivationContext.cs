using System;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface IDeactivationContext
	{
		IServiceProvider ServiceProvider { get; }

		bool Cancelled { get; }

		void Cancel();
	}
}