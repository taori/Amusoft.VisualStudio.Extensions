using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public interface IActivateable
	{
		bool Activated { get; }

		Task ActivateAsync(IActivationContext context);

		event AsyncEventHandler OnActivated;
	}
}