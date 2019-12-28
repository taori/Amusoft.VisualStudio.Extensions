using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Interactivity
{
	public interface IDeactivate
	{
		Task DeactivateAsync(IDeactivationContext context);

		event AsyncEventHandler OnDeactivated;
	}
}