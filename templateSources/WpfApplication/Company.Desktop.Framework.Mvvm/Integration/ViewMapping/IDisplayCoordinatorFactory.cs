namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public interface IDisplayCoordinatorFactory
	{
		IDisplayCoordinator Create(object dataContext);
	}
}