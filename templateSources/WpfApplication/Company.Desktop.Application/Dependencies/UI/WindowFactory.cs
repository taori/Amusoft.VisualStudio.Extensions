using System;
using System.Windows;
using Company.Desktop.Application.Views.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.ViewModels.Windows;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class WindowFactory : IViewModelWindowFactory
	{
		/// <inheritdoc />
		public bool CanCreateWindow(object dataContext)
		{
			if (dataContext == null)
			{
				throw new ArgumentNullException(nameof(dataContext), nameof(dataContext));
			}

			return true;
		}

		/// <inheritdoc />
		public Window CreateWindow(object dataContext)
		{
			if(dataContext is DefaultWindowViewModel)
				return new DefaultWindow();

			return new DefaultWindow();
//			if(dataContext is MainViewModel)
//				return new MainWindow();
//
//			var defaultWindow = new DefaultWindow();
//			return defaultWindow;
		}
	}
}