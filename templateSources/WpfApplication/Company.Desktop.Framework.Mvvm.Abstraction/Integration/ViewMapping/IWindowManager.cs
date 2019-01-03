using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface IWindowManager
	{
		void RegisterWindow(Window window, string id);
		bool TryGetWindow(string id, out Window window);
	}
}