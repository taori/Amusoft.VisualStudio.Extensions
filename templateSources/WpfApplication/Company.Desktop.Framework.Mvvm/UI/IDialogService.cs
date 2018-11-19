using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public interface IDialogService
	{
		Task DisplayMessageAsync(string title, string message);
		Task<bool> ConfirmAsync(string question);
	}
}