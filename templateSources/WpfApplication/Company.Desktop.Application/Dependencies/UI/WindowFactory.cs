using System;
using System.Windows;
using Company.Desktop.Application.Views.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.ViewModels.Windows;
using NLog;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class WindowFactory : IViewModelWindowFactory
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowFactory));

		/// <inheritdoc />
		public bool CanCreateWindow(IActivateable activateable)
		{
			if (activateable == null)
			{
				throw new ArgumentNullException(nameof(activateable), nameof(activateable));
			}

			return true;
		}

		/// <inheritdoc />
		public Window CreateWindow(IActivateable activateable)
		{
			if(activateable is MainViewModel)
				return new MainWindow();

			return new DefaultWindow();
		}
	}
}