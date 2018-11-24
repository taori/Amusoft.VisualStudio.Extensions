using System;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowViewModel : IActivateable, IDisposable, IWindowListener
	{
		string Title { get; set; }
		double Width { get; set; }
		double MinWidth { get; set; }
		double MaxWidth { get; set; }
		double Height { get; set; }
		double MinHeight { get; set; }
		double MaxHeight { get; set; }

		IContentViewModel Content { get; set; }
		
		bool ClaimMainWindowOnOpen { get; }

		ResizeMode ResizeMode { get; set; }

		void Focus();
		void Normalize();
		void Maximize();
		void Minimize();
		void Close();

		IObservable<object> WhenFocusRequested { get; }
		IObservable<object> WhenClosingRequested { get; }
		IObservable<object> WhenNormalizeRequested { get; }
		IObservable<object> WhenMinimizeRequested { get; }
		IObservable<object> WhenMaximizeRequested { get; }
	}
}