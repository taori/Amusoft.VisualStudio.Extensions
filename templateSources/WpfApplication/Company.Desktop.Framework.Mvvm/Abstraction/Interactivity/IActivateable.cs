using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IActivateable
	{
		bool Activated { get; }

		Task ActivateAsync(IActivationContext context);

		event AsyncEventHandler OnActivated;
	}
}