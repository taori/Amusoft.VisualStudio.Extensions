using System.Windows.Input;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public interface IWindowCommand
	{
		ICommand Command { get; set; }
	}
}