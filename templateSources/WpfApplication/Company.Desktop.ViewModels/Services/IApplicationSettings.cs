namespace Company.Desktop.ViewModels.Services
{
	public interface IApplicationSettings
	{
		void Update();
		bool FocusTabOnCreate { get; set; }
		bool FocusTabOnOpen { get; set; }
	}
}