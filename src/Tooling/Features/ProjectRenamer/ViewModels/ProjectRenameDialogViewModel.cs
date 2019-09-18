using System;
using System.Globalization;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Tooling.Dependencies;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Utility;
using Tooling.Shared;
using Tooling.Shared.Resources;
using Tooling.Utility;

namespace Tooling.Features.ProjectRenamer.ViewModels
{
	public class ValidFileNameValidationRule : ValidationRule
	{
		/// <inheritdoc />
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var s = value?.ToString();
			if(string.IsNullOrEmpty(s))
				return new ValidationResult(false, "Value cannot be empty.");

			return ValidationResult.ValidResult;
		}
	}

	public class ProjectRenameDialogViewModel : ViewModelBase, IDisposable
	{
		private readonly CompositeDisposable _composables = new CompositeDisposable();

		private Project _project;

		public Project Project
		{
			get => _project;
			set => SetValue(ref _project, value, nameof(Project));
		}

		private string _oldProjectName;

		public string OldProjectName
		{
			get => _oldProjectName;
			set => SetValue(ref _oldProjectName, value, nameof(OldProjectName));
		}

		private AttributedValue<string> _newProjectName;

		public AttributedValue<string> NewProjectName
		{
			get => _newProjectName;
			set => SetValue(ref _newProjectName, value, nameof(NewProjectName));
		}

		private bool _isViewModelTerminated;

		public bool IsViewModelTerminated
		{
			get => _isViewModelTerminated;
			set => SetValue(ref _isViewModelTerminated, value, nameof(IsViewModelTerminated));
		}

		private string _oldProjectPath;

		public string OldProjectPath
		{
			get => _oldProjectPath;
			set => SetValue(ref _oldProjectPath, value, nameof(OldProjectPath));
		}
		
		public string NewProjectPath
		{
			get
			{
				var parentFolder = new Uri(new Uri(OldProjectPath, UriKind.Absolute), new Uri(@"..", UriKind.Relative)).AbsolutePath;
				var ext = Path.GetExtension(OldProjectPath);
				var newName = Path.Combine(parentFolder.Replace('/', Path.DirectorySeparatorChar), NewProjectName.Value, $"{NewProjectName.Value}{ext}");
				return newName;
			}
			set {  }
		}

		private ICommand _updateCommand;

		public ICommand UpdateCommand
		{
			get => _updateCommand;
			set => SetValue(ref _updateCommand, value, nameof(UpdateCommand));
		}

		public ProjectRenameDialogViewModel()
		{
			UpdateCommand = new RelayCommand(UpdateExecute, d => IsNameValid(_newProjectName?.Value, out _));

			_composables.Add(WhenPropertyChanged
				.Where(d => d == nameof(NewProjectName))
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(OnNewNameChanged));

			_composables.Add(WhenPropertyChanged
				.Where(d => d == nameof(Project))
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(OnProjectChanged));
		}

		private void OnProjectChanged(string obj)
		{
			if (Project == null)
				IsViewModelTerminated = true;
		}

		private readonly Regex _projectNameExpression = new Regex(@"^(?!\d)[\w\d\._]{3,}$", RegexOptions.Compiled);
		private void OnNewNameChanged(string name)
		{
			var newName = _newProjectName?.Value;
			if (!IsNameValid(newName, out var error))
			{
				NewProjectName.Message = error;
			}
			else
			{
				NewProjectName.Message = null;

				OnPropertyChanged(nameof(NewProjectPath));
			}
		}

		private bool IsNameValid(string name, out string error)
		{
			if (string.IsNullOrEmpty(name))
			{
				error = "Project name can not be empty.";
				return false;
			}

			if (new Regex(@"^\d").IsMatch(name))
			{
				error = $"The name cannot start with a number.";
				return false;
			}

			if (name.Length < 3)
			{
				error = $"The new name is too short.";
				return false;
			}

			if (!_projectNameExpression.IsMatch(name))
			{
				error = $"The name can only cantain alphanumeric characters, dots and underscore.";
				return false;
			}

			error = null;
			return true;
		}

		private async void UpdateExecute(object obj)
		{
			if (!await SolutionHelper.IsIdeAndSolutionFileInSyncAsync())
			{
				if (MessageBox.Show(Translations.msg_theSolutionMustBeSavedConfirm, Translations.caption_Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
					return;

				SolutionHelper.SaveSolution();
				await Task.Delay(1000);
			}

			var targetUri = new Uri(new Uri(OldProjectPath, UriKind.Absolute), new Uri("..", UriKind.Relative))
				.AbsolutePath.Replace('/', Path.DirectorySeparatorChar);
			var options = new MoverToolOptions();
			options.ProjectPathTransformer = new SingleProjectRenamer(OldProjectName, NewProjectName.Value);

			var tool = new MoverTool(new[] {OldProjectPath}, SolutionHelper.GetActiveIDE().Solution.FileName, targetUri, options);
			await tool.MoveAsync();
			Project = null;
		}

		public ProjectRenameDialogViewModel(Project project) : this()
		{
			Project = project;
			OldProjectPath = project.FullName;
			OldProjectName = Path.GetFileNameWithoutExtension(project.FullName);
			NewProjectName = Path.GetFileNameWithoutExtension(project.FullName);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_composables?.Dispose();
		}
	}
}