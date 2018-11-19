using System;
using Company.Desktop.Framework.Extensibility;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class WindowCloseContext : IWindowCloseBehaviourContext
	{
		/// <inheritdoc />
		public WindowCloseContext(object viewModel, IServiceProvider serviceProvider)
		{
			ViewModel = viewModel;
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public object ViewModel { get; }

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