using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Shared.Resources;

namespace Company.Desktop.Application.Dependencies.UI
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