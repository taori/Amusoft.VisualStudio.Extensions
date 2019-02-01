namespace Company.Desktop.Framework.Mvvm.Abstraction.ViewModel
{
	/// <summary>
	/// Interface to apply configuration when the <see cref="IWindowViewModel"/> is applied to the window
	/// </summary>
	public interface IConfigureWindow
	{
		void Configure(IWindowViewModel window);
	}
}