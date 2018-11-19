using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Application.Resources;
using Company.Desktop.Framework.Mvvm.UI;

namespace Company.Desktop.Application.Shell.Dependencies
{
	public class DialogService : IDialogService
	{
		/// <inheritdoc />
		public Task DisplayMessageAsync(string title, string message)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<bool> ConfirmAsync(string question)
		{
			if (MessageBox.Show(question, Translations.shared_Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				return Task.FromResult(true);

			return Task.FromResult(false);
		}
	}
}