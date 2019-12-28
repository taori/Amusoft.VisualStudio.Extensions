using System;

namespace Company.Desktop.Framework.Mvvm.Interactivity
{
	public class DeactivationContext : IDeactivationContext
	{
		/// <inheritdoc />
		public DeactivationContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public bool Cancelled { get; private set; }

		/// <inheritdoc />
		public void Cancel()
		{
			Cancelled = true;
		}
	}
}