using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Window
{
	public class WindowClosingContext : IWindowClosingBehaviorContext
	{
		/// <inheritdoc />
		public WindowClosingContext(object viewModel, IServiceProvider serviceProvider)
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