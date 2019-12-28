using System;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.UI;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public interface IWindowViewModel : IDisposable, IWindowListener, IActivateable
	{
		string Title { get; set; }
		double Width { get; set; }
		double MinWidth { get; set; }
		double MaxWidth { get; set; }
		double Height { get; set; }
		double MinHeight { get; set; }
		double MaxHeight { get; set; }

		IWindowContentViewModel Content { get; }

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

		SizeToContent SizeToContent { get; set; }
		ResizeMode ResizeMode { get; set; }
		bool ShowInTaskbar { get; set; }
		double Top { get; set; }
		double Left { get; set; }
	}
}