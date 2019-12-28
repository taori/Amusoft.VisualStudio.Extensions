using System;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.UI;
using Company.Desktop.Shared.Resources;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class SystemDialogService : IDialogService
	{
		/// <inheritdoc />
		public Task DisplayMessageAsync(object owner, string title, string message)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<bool> YesNoAsync(object owner, string message, string title = null)
		{
			if (MessageBox.Show(message, Translations.shared_Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				return Task.FromResult(true);

			return Task.FromResult(false);
		}

		/// <inheritdoc />
		public Task<bool?> YesNoCancelAsync(object owner, string message, string title = null)
		{
			var result = MessageBox.Show(message, Translations.shared_Question, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
			switch (result)
			{
				case MessageBoxResult.Cancel:
					return Task.FromResult((bool?) null);
				case MessageBoxResult.Yes:
					return Task.FromResult((bool?) true);
				case MessageBoxResult.No:
					return Task.FromResult((bool?) false);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <inheritdoc />
		public async Task<string> GetTextAsync(object owner, string message, string title)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public async Task<IProgressController> ShowProgressAsync(object owner, string title, string message, bool cancelable, Action<IProgressController> abortion)
		{
			throw new NotImplementedException();
		}
	}
}