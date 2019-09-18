using System;
using System.Globalization;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EnvDTE;
using Tooling.Shared;
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

		public Project Project { get; private set; }

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
			UpdateCommand = new RelayCommand(UpdateExecute);

			_composables.Add(WhenPropertyChanged
				.Where(d => Test(d))
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(OnNewNameChanged));
		}

		private bool Test(string d)
		{
			return d == nameof(NewProjectName);
		}

		private readonly Regex _projectNameExpression = new Regex(@"^(?!\d)[\w\d\._]{3,}$", RegexOptions.Compiled);
		private void OnNewNameChanged(string name)
		{
			var newName = _newProjectName?.Value;
			if (string.IsNullOrEmpty(newName))
			{
				NewProjectName.Message = "Project name can not be empty.";
				return;
			}

			if (new Regex(@"^\d").IsMatch(newName))
			{
				NewProjectName.Message = $"The name cannot start with a number.";
				return;
			}

			if (newName.Length < 3)
			{
				NewProjectName.Message = $"The new name is too short.";
				return;
			}

			if (!_projectNameExpression.IsMatch(newName))
			{
				NewProjectName.Message = $"The name can only cantain alphanumeric characters, dots and underscore.";
				return;
			}

			NewProjectName.Message = string.Empty;
				
			OnPropertyChanged(nameof(NewProjectPath));
		}

		private void UpdateExecute(object obj)
		{
			MessageBox.Show($"The new path is {NewProjectPath}");
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