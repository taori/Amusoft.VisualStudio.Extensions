using System;
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

		void RequestFocus();

		event EventHandler FocusRequested;
	}
}