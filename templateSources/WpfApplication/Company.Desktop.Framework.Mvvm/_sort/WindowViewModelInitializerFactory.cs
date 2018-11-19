using System;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm._sort
{
	public class WindowViewModelInitializerFactory : IViewModelInitializerFactory
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowViewModelInitializerFactory));

		public ViewModelInitializerContext Context { get; }

		public WindowViewModelInitializerFactory(ViewModelInitializerContext context)
		{
			Context = context;
		}

		/// <inheritdoc />
		public IViewModelInitializer Create(IActivateable activateable)
		{
			var windowFactories = Context.ServiceProvider.GetServices<IViewModelWindowFactory>();
			Log.Debug($"Looking for implementations of {nameof(IViewModelWindowFactory)} to create a window initializer for [{activateable.GetType().FullName}].");
			foreach (var factory in windowFactories)
			{
				if (factory.CanCreateWindow(activateable))
				{
					Log.Debug($"Matching factory found -> Creating window for [{activateable.GetType().FullName}]");
					var window = factory.CreateWindow(activateable);
					return new WindowInitializer<WindowViewModel>(Context, window, activateable as WindowViewModel);
				}
			}

			Log.Error($"There is no implementation of  {nameof(IViewModelWindowFactory)} to to handle [{activateable.GetType().FullName}].");
			return null;
		}

		/// <inheritdoc />
		public IViewModelInitializer Create(IActivateable activateable, object view)
		{
			if(view is Window window)
				return new WindowInitializer<WindowViewModel>(Context, window, activateable as WindowViewModel);

			throw new ArgumentException($"{nameof(view)} must be of type {nameof(Window)}.", nameof(view));
		}

		/// <inheritdoc />
		public bool CanHandle(IActivateable activateable)
		{
			if (activateable is WindowViewModel)
				return true;

			return false;
		}
	}
}