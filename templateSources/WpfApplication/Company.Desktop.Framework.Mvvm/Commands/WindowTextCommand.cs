using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.Framework.Mvvm.Commands
{

	public class WindowTextCommand : ViewModelBase, IWindowCommand
	{
		public WindowTextCommand(ICommand command, string text)
		{
			Command = command;
			Text = text;
		}

		private ICommand _command;

		public ICommand Command
		{
			get { return _command; }
			set { SetValue(ref _command, value, nameof(Command)); }
		}

		private string _text;

		public string Text
		{
			get { return _text; }
			set { SetValue(ref _text, value, nameof(Text)); }
		}
	}
}