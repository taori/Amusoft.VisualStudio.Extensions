using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowContentViewModel : IContentViewModel, IDefaultBehaviourProvider
	{
		IWindowViewModel Window { get; }
		bool ClaimMainWindowOnOpen { get; }
		string GetTitle();
	}
}