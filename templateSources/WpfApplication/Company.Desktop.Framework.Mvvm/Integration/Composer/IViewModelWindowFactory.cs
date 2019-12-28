using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public interface IViewModelWindowFactory
	{
		bool CanCreateWindow(object dataContext);
		Window CreateWindow(object dataContext);
	}
}