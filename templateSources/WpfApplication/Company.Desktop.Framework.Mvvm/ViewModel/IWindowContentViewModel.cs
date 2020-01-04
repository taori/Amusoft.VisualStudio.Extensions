using System.Collections.ObjectModel;
using Company.Desktop.Framework.Mvvm.Interactivity;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public interface IWindowContentViewModel : IContentViewModel, IDefaultBehaviorProvider
	{
		ObservableCollection<IWindowCommand> LeftWindowCommands { get; }
		ObservableCollection<IWindowCommand> RightWindowCommands { get; }
		IWindowViewModel Window { get; }
		bool ClaimMainWindowOnOpen { get; }
		string GetTitle();
	}
}