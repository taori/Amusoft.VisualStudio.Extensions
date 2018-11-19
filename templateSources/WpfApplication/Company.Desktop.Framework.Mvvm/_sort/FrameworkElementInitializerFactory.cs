using System;
using System.Windows;
using System.Windows.Controls;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.Framework.Mvvm._sort
{
	public class FrameworkElementInitializerFactory : IViewModelInitializerFactory
	{
		public ViewModelInitializerContext Context { get; }

		public FrameworkElementInitializerFactory(ViewModelInitializerContext context)
		{
			Context = context;
		}

		/// <inheritdoc />
		public IViewModelInitializer Create(IActivateable activateable)
		{
			return new FrameworkElementInitializer<ContentViewModel>(Context, new UserControl(), activateable as ContentViewModel);
		}

		/// <inheritdoc />
		public IViewModelInitializer Create(IActivateable activateable, object view)
		{
			if (view is FrameworkElement element)
				return new FrameworkElementInitializer<ContentViewModel>(Context, element, activateable as ContentViewModel);

			throw new ArgumentException($"{nameof(view)} must be of type {nameof(FrameworkElement)}.", nameof(view));
		}

		/// <inheritdoc />
		public bool CanHandle(IActivateable activateable)
		{
			if (activateable is ContentViewModel)
				return true;
			return false;
		}
	}
}