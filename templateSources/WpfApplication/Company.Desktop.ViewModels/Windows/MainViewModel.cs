using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Amusoft.UI.WPF.Adorners;
using Amusoft.UI.WPF.Notifications;
using Company.Desktop.Framework.Extensions;
using Company.Desktop.Framework.Mvvm.Commands;
using Company.Desktop.Framework.Mvvm.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.UI;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.ViewModels.Common;
using Company.Desktop.ViewModels.Controls;
using Microsoft.Extensions.DependencyInjection;
using IBehavior = Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors.IBehavior;

namespace Company.Desktop.ViewModels.Windows
{
	public class MainViewModel : WindowContentViewModelBase
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

		public class TestViewModel : ViewModelBase
		{
			private readonly INavigationService _navigationService;

			public TestViewModel(INavigationService navigationService)
			{
				_navigationService = navigationService;
			}
		}

		/// <inheritdoc />
		protected override Task OnActivateAsync(IActivationContext context)
		{
			var composer = ServiceProvider.GetRequiredService<IViewModelComposer>();
			var testViewModel = composer.Compose<TestViewModel>();
			var scopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
			this.RightWindowCommands.Add(new WindowTextCommand(new TaskCommand(OpenSettingsExecute), "Settings"));
			var disableBehavior = new DisableWhileExecutingCommand();
			Commands.Add(new TestCommand("Open Window", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				var navigation = ServiceProvider.GetRequiredService<INavigationService>();
				var dialogService = ServiceProvider.GetRequiredService<IDialogService>();

				var viewModel = new SecondaryWindowViewModel();

				var windowViewModel = new DefaultWindowViewModel(viewModel);
				windowViewModel.Behaviors.Add(new ClosingDelegateInterceptorBehavior(c => RequestSecondaryWindowClose(windowViewModel, c)));

				if (await dialogService.YesNoAsync(this, "Should this window be opened with an ID?"))
					await navigation.OpenWindowAsync(windowViewModel, "secondWindow");
				else
					await navigation.OpenWindowAsync(windowViewModel, null);

				windowViewModel.Focus();
			}))));

			Commands.Add(new TestCommand("Open Dialog", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				var dialogService = ServiceProvider.GetRequiredService<IDialogService>();
				//				await dialogService.DisplayMessageAsync(this, "title", "message");
				//				await dialogService.YesNoAsync(this, "YesNo");
				//				await dialogService.YesNoCancelAsync(this, "YesNoCancel");
//				var text = await dialogService.GetTextAsync(this, "GetText", "GetTextTitle");
//				await dialogService.DisplayMessageAsync(this, text, "Notice");
				var controller = await dialogService.ShowProgressAsync(this, "GetText", "GetTextTitle", true, async (progressController) =>
				{
					await progressController.CloseAsync();
					await dialogService.DisplayMessageAsync(this, "", "aborted");
				});
				controller.Minimum = 0;
				controller.Maximum = 100;
				await Task.Run(async () =>
				{
					for (int i = 0; i < 100; i++)
					{
						if (controller.IsCanceled)
							return;
						controller.SetProgress(i);
						await Task.Delay(20);
					}

					await controller.CloseAsync();
				});
			}))));

			Commands.Add(new TestCommand("Update top area", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				var r = new Random();
				var vm = new SampleDataOverviewViewModel(r.Next(10, 30));
				vm.Behaviors.Add(new ConfirmContentChangingBehavior());
				var opened = await UpdateRegionAsync(vm, RegionNames.TopArea);
			}))));

			Commands.Add(new TestCommand("Update bottom area", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				var r = new Random();
				var vm = new SampleDataOverviewViewModel(r.Next(10, 30));
				var opened = await UpdateRegionAsync(vm, RegionNames.BottomArea);
			}))));

			Commands.Add(new TestCommand("Spawn notifications", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				if (ServiceProvider.TryGetService(out INotificationService notificationService))
				{
					for (int i = 0; i < 5; i++)
					{
						await Task.Delay(800);
						var notification = Notification.PrimaryScreen($"Test title {i}.", $"Just testmessage {i}.")
							.AutoClose(TimeSpan.FromSeconds(5))
							.CloseOnSelect(true);
						notificationService.Display(notification.Notification);
					}
				}
			}))));

			Commands.Add(new TestCommand("Test Composition", new CompositionCommand(disableBehavior, new TaskExecution(CommandHookCallback), new DisableWhileExecutingCommand())));
			Commands.Add(new TestCommand("Spawn error", new CompositionCommand(disableBehavior, new TaskExecution(SpawnErrorExecute), new DisableWhileExecutingCommand())));

			Commands.Add(new TestCommand("Run GC", new CompositionCommand(disableBehavior, new TaskExecution(parameter =>
			{
				GC.Collect();
				return Task.CompletedTask;
			}))));

			return Task.CompletedTask;
		}

		private async Task OpenSettingsExecute(object arg)
		{
			var navigationService = this.ServiceProvider.GetRequiredService<INavigationService>();
			await navigationService.OpenWindowAsync(new DefaultWindowViewModel(new SettingsViewModel()), null);
		}

		private Task SpawnErrorExecute(object parameter)
		{
			Log.Error("this is just a test");
			return Task.CompletedTask;
		}

		private async Task<bool> RequestSecondaryWindowClose(DefaultWindowViewModel windowViewModel, IWindowClosingBehaviorContext context)
		{
			if (ServiceProvider.TryGetService(out IDialogService dialogService))
			{
				return await dialogService.YesNoAsync(windowViewModel, "Close for sure?");
			}

			return true;
		}

		private async Task CommandHookCallback(object parameter)
		{
			Log.Debug($"Command hook executing.");
			await Task.Delay(3000);
		}

		/// <inheritdoc />
		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield return new RequestClosingPermissionBehavior();
			yield return new RestoreWindowDimensionsBehavior();
		}

		/// <inheritdoc />
		public override string GetTitle()
		{
			return "Application name";
		}
	}
}