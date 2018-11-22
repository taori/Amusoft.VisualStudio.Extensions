using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.UI;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.ViewModels.Common;
using Company.Desktop.ViewModels.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.ViewModels.Windows
{
	public class MainViewModel : WindowViewModel
	{
		private ObservableCollection<TestCommand> _commands = new ObservableCollection<TestCommand>();

		public ObservableCollection<TestCommand> Commands
		{
			get => _commands;
			set => SetValue(ref _commands, value, nameof(Commands));
		}

		public class TestCommand
		{
			public string Text { get; set; }

			public ICommand Command { get; set; }

			/// <inheritdoc />
			public TestCommand(string text, ICommand command)
			{
				Text = text;
				Command = command;
			}
		}
		
		/// <inheritdoc />
		protected override Task OnActivateAsync(IActivationContext context)
		{
			Commands.Add(new TestCommand("Open Window", new RelayCommand(async (o) =>
			{
				var navigation = ServiceProvider.GetRequiredService<INavigationService>();
				var dialogService = ServiceProvider.GetRequiredService<IDialogService>();

				var viewModel = new SecondaryWindowViewModel();
				viewModel.Behaviours.Add(new RequestClosePermissionBehaviour());
				viewModel.WhenClosed.Subscribe(d =>
				{
					Log.Info($"Window has been closed");
					viewModel.Dispose();
				});

				if (await dialogService.ConfirmAsync("Should this window be opened with an ID?"))
					await navigation.OpenWindowAsync(viewModel, "secondWindow");
				else
					await navigation.OpenWindowAsync(viewModel, null);
			})));

			Commands.Add(new TestCommand("Update top area", new RelayCommand(async (o) =>
			{
				var r = new Random();
				var opened = await UpdateRegionAsync(new SampleDataOverviewViewModel(r.Next(10, 30)), RegionNames.TopArea);
			})));

			Commands.Add(new TestCommand("Update bottom area", new RelayCommand(async (o) =>
			{
				var r = new Random();
				var opened = await UpdateRegionAsync(new SampleDataOverviewViewModel(r.Next(10, 30)), RegionNames.BottomArea);
			})));

			Commands.Add(new TestCommand("Run GC", new RelayCommand((o) =>
			{
				GC.Collect();
			})));
			
			return Task.CompletedTask;
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