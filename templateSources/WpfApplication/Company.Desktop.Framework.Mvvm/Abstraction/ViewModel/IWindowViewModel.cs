using System;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowViewModel : IActivateable
	{
		string Title { get; set; }
		double Width { get; set; }
		double Height { get; set; }

		IContentViewModel Content { get; set; }
		
		bool ClaimMainWindowOnOpen { get; }

		ResizeMode ResizeMode { get; set; }

		void RequestFocus();

		event EventHandler FocusRequested;
	}
}