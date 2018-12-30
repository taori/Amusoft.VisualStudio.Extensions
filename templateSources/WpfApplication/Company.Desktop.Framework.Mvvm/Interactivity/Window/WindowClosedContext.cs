using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Window
{
	public class WindowClosedContext : IWindowClosedBehaviorContext
	{
		/// <inheritdoc />
		public WindowClosedContext([NotNull] object viewModel, [NotNull] IServiceProvider serviceProvider, [NotNull] IViewCompositionContext context)
		{
			ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			Context = context ?? throw new ArgumentNullException(nameof(context));
		}

		/// <inheritdoc />
		public object ViewModel { get; }

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		public IViewCompositionContext Context { get; }
	}
}