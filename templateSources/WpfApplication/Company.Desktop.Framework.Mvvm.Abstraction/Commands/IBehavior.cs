using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Commands
{
	public interface IBehavior
	{
		bool CanExecute(object parameter);
		Task ExecuteAsync(object parameter);
		Task AllExecutedAsync(object parameter);
		Task AllExecutingAsync(object parameter);
		Task OnExecutingAsync(object parameter);
		Task OnExecutedAsync(object parameter);
	}
}