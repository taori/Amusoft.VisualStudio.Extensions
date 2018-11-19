using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface IDeactivate
	{
		Task DeactivateAsync(IDeactivationContext context);

		event AsyncEventHandler OnDeactivated;
	}
}