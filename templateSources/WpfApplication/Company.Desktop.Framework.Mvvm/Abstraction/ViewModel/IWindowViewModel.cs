using System;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowViewModel : IActivateable, IDisposable
	{
		string Title { get; set; }
		double Width { get; set; }
		double Height { get; set; }

		IContentViewModel Content { get; set; }
		
		bool ClaimMainWindowOnOpen { get; }

		ResizeMode ResizeMode { get; set; }

		void Focus();
		void Normalize();
		void Maximize();
		void Minimize();
		void Close();

		IObservable<object> FocusRequested { get; }
		IObservable<object> CloseRequested { get; }
		IObservable<object> NormalizeRequested { get; }
		IObservable<object> MinimizeRequested { get; }
		IObservable<object> MaximizeRequested { get; }
	}
}