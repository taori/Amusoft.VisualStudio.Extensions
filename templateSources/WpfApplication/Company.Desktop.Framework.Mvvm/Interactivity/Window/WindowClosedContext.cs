using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Window
{
	public class WindowClosedContext : IWindowClosedBehaviourContext
	{
		/// <inheritdoc />
		public WindowClosedContext(object viewModel, IServiceProvider serviceProvider)
		{
			ViewModel = viewModel;
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public object ViewModel { get; }

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }
	}
}