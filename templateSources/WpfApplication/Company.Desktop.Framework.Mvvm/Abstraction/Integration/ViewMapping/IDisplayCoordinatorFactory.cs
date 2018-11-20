using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface IDisplayCoordinatorFactory
	{
		IDisplayCoordinator Create(object dataContext);
	}
}