using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Commands;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.ViewModels.Common;
using Company.Desktop.ViewModels.Controls;
using Microsoft.Extensions.DependencyInjection;

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

		/// <inheritdoc />
		protected override Task OnActivateAsync(IActivationContext context)
		{
			var disableBehavior = new DisableWhileExecutingBehavior();
			Commands.Add(new TestCommand("Open Window", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				var navigation = ServiceProvider.GetRequiredService<INavigationService>();
				var dialogService = ServiceProvider.GetRequiredService<IDialogService>();

				var viewModel = new SecondaryWindowViewModel();

				if (await dialogService.YesNoAsync(this, "Should this window be opened with an ID?"))
					await navigation.OpenWindowAsync(new DefaultWindowViewModel(viewModel), "secondWindow");
				else
					await navigation.OpenWindowAsync(new DefaultWindowViewModel(viewModel), null);
			}))));

			Commands.Add(new TestCommand("Open Dialog", new CompositionCommand(disableBehavior, new TaskExecution(async (o) =>
			{
				var dialogService = ServiceProvider.GetRequiredService<IDialogService>();
//				await dialogService.DisplayMessageAsync(this, "title", "message");
//				await dialogService.YesNoAsync(this, "YesNo");
//				await dialogService.YesNoCancelAsync(this, "YesNoCancel");
//				await dialogService.GetTextAsync(this, "GetText", "GetTextTitle");
				var controller = await dialogService.ShowProgressAsync(this, "GetText", "GetTextTitle", true, async(progressController) =>
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

			Commands.Add(new TestCommand("Test Composition", new CompositionCommand(disableBehavior, new TaskExecution(CommandHookCallback), new DisableWhileExecutingBehavior())));

			Commands.Add(new TestCommand("Run GC", new CompositionCommand(disableBehavior, new TaskExecution(parameter =>
			{
				GC.Collect();
				return Task.CompletedTask;
			}))));
			
			return Task.CompletedTask;
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