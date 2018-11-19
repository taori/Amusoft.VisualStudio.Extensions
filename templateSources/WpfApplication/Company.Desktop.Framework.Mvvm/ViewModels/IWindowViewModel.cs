using Company.Desktop.Framework.Mvvm.Navigation;

namespace Company.Desktop.Framework.Mvvm.ViewModels
{
	public interface IWindowViewModel : IActivateable
	{
		string Title { get; set; }
		double Width { get; set; }
		double Height { get; set; }

		IContentViewModel Content { get; set; }
		
		bool ClaimMainWindowOnOpen { get; }
	}
}