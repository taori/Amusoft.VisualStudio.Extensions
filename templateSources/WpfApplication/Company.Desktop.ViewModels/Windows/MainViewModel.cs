using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.UI;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.ViewModels.Common;
using Company.Desktop.ViewModels.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.ViewModels.Windows
{
	public class MainViewModel : WindowViewModel
	{
		private ICommand _openWindowCommand;

		public ICommand OpenWindowCommand
		{
			get => _openWindowCommand;
			set => SetValue(ref _openWindowCommand, value, nameof(OpenWindowCommand));
		}

		private ICommand _updateBottomAreaCommand;

		public ICommand UpdateBottomAreaCommand
		{
			get => _updateBottomAreaCommand;
			set => SetValue(ref _updateBottomAreaCommand, value, nameof(UpdateBottomAreaCommand));
		}

		private ICommand _updateTopAreaCommand;

		public ICommand UpdateTopAreaCommand
		{
			get => _updateTopAreaCommand;
			set => SetValue(ref _updateTopAreaCommand, value, nameof(UpdateTopAreaCommand));
		}

		/// <inheritdoc />
		protected override Task OnActivateAsync(IActivationContext context)
		{
			OpenWindowCommand = new RelayCommand(OpenWindowExecute);
			UpdateBottomAreaCommand = new RelayCommand(UpdateBottomAreaExecute);
			UpdateTopAreaCommand = new RelayCommand(UpdateTopAreaExecute);

			return Task.CompletedTask;
		}

		private async void UpdateTopAreaExecute(object obj)
		{
			var r = new Random();
			await UpdateRegionAsync(new SampleDataOverviewViewModel(r.Next(10, 30)), RegionNames.TopArea);
		}

		private async void UpdateBottomAreaExecute(object obj)
		{
			var r = new Random();
			await UpdateRegionAsync(new SampleDataOverviewViewModel(r.Next(10, 30)), RegionNames.BottomArea);
		}

		private async void OpenWindowExecute(object obj)
		{
			var navigation = ServiceProvider.GetRequiredService<INavigationService>();
			var dialogService = ServiceProvider.GetRequiredService<IDialogService>();

			if(await dialogService.ConfirmAsync("Should this window be opened with an ID?"))
				await navigation.OpenWindowAsync(new SecondaryWindowViewModel(), "secondWindow");
			else
				await navigation.OpenWindowAsync(new SecondaryWindowViewModel(), null);
		}

		/// <inheritdoc />
		protected override string GetWindowTitle()
		{
			return "Application name";
		}

		/// <inheritdoc />
		public override bool ClaimMainWindowOnOpen => true;

		/// <inheritdoc />
		protected override void InitializeBehaviours()
		{
			
		}
	}
}