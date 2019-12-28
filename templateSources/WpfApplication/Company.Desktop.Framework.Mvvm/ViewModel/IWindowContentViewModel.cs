using Company.Desktop.Framework.Mvvm.Interactivity;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public interface IWindowContentViewModel : IContentViewModel, IDefaultBehaviorProvider
	{
		IWindowViewModel Window { get; }
		bool ClaimMainWindowOnOpen { get; }
		string GetTitle();
	}
}