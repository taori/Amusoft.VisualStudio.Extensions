using System;

namespace Company.Desktop.Framework.Mvvm.Interactivity
{
	public interface IDeactivationContext
	{
		IServiceProvider ServiceProvider { get; }

		bool Cancelled { get; }

		void Cancel();
	}
}