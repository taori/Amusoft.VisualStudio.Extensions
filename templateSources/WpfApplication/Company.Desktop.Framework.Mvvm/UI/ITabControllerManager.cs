namespace Company.Desktop.Framework.Mvvm.UI
{
	public interface ITabControllerManager
	{
		bool TryGetController(object viewModel, string name, out ITabController controller);
	}
}