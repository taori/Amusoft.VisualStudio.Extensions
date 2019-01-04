using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Shared.Resources;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class MetroDialogService : IDialogService
	{
		public IRegionManager RegionManager { get; }

		private static readonly ILogger Log = LogManager.GetLogger(nameof(MetroDialogService));

		public MetroDialogService(IRegionManager regionManager)
		{
			RegionManager = regionManager;
		}

		protected bool TryGetWindowByOwner(object owner, out MetroWindow window)
		{
			var first = System.Windows.Application.Current.Windows.Cast<Window>().FirstOrDefault(d => ReferenceEquals(d.DataContext, owner));
			if (first is MetroWindow metroWindow)
			{
				window = metroWindow;
				return true;
			}

			if (RegionManager.GetHostingWindow(owner) is MetroWindow regionManagerWindow)
			{
				window = regionManagerWindow;
				return true;
			}

			Log.Error($"Unable to locate matching {nameof(MetroWindow)}.");
			window = null;
			return false;
		}

		/// <inheritdoc />
		public async Task DisplayMessageAsync(object owner, string title, string message)
		{
			if (!TryGetWindowByOwner(owner, out var window))
				return;

			await window.ShowMessageAsync(title, message);
		}

		/// <inheritdoc />
		public async Task<bool> YesNoAsync(object owner, string message, string title = null)
		{
			if (!TryGetWindowByOwner(owner, out var window))
				return false;

			var settings = new MetroDialogSettings();
			settings.AffirmativeButtonText = Translations.shared_Yes;
			settings.NegativeButtonText = Translations.shared_No;

			return await window.ShowMessageAsync(
					   title ?? Translations.shared_Question,
					   message,
					   MessageDialogStyle.AffirmativeAndNegative,
					   settings: settings) == MessageDialogResult.Affirmative;
		}

		/// <inheritdoc />
		public async Task<bool?> YesNoCancelAsync(object owner, string message, string title = null)
		{
			if (!TryGetWindowByOwner(owner, out var window))
				return false;

			var settings = new MetroDialogSettings();
			settings.AffirmativeButtonText = Translations.shared_Yes;
			settings.NegativeButtonText = Translations.shared_No;
			settings.FirstAuxiliaryButtonText = Translations.shared_Cancel;

			var result = await window.ShowMessageAsync(
				title ?? Translations.shared_Question,
				message,
				MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
				settings: settings);

			switch (result)
			{
				case MessageDialogResult.Affirmative:
					return true;
				case MessageDialogResult.Negative:
					return false;
				case MessageDialogResult.FirstAuxiliary:
					return null;
				default:
					throw new NotSupportedException();
			}
		}

		/// <inheritdoc />
		public async Task<string> GetTextAsync(object owner, string title, string message)
		{
			if (!TryGetWindowByOwner(owner, out var window))
				return null;
			
			var settings = new MetroDialogSettings();
			settings.AffirmativeButtonText = Translations.shared_OK;
			settings.NegativeButtonText = Translations.shared_Cancel;

			return await window.ShowInputAsync(title, message, settings);
		}

		/// <inheritdoc />
		public async Task<IProgressController> ShowProgressAsync(object owner, string title, string message, bool cancelable, Action<IProgressController> abortion)
		{
			if (!TryGetWindowByOwner(owner, out var window))
				return null;
			
			var metroController = await window.ShowProgressAsync(title, message, cancelable);
			var controller = new MetroProgressController(metroController);
			controller.SetMessage(message);
			controller.SetTitle(title);
			controller.WhenCanceled.Subscribe(args => abortion(controller));
			return controller;
		}
	}
}