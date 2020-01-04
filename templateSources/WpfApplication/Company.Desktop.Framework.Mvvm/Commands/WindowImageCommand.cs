using System.Windows.Input;
using System.Windows.Media;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.Framework.Mvvm.Commands
{
	public class WindowImageCommand : IWindowCommand
	{
		public WindowImageCommand(ICommand command, ImageSource imageSource)
		{
			Command = command;
			ImageSource = imageSource;
		}

		public ICommand Command { get; set; }

		public ImageSource ImageSource { get; set; }
	}
}