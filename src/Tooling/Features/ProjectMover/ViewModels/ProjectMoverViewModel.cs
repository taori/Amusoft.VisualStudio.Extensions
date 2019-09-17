using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using EnvDTE80;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Tooling.Shared;
using Tooling.Shared.Resources;
using Tooling.Utility;

namespace Tooling.Features.ProjectMover.ViewModels
{
	public class ProjectMoverViewModel : ViewModelBase, IDisposable
	{
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		private ObservableCollection<ProjectMoverItemViewModel> _projects;

		public ObservableCollection<ProjectMoverItemViewModel> Projects
		{
			get => _projects ?? (_projects = new ObservableCollection<ProjectMoverItemViewModel>());
			set => SetValue(ref _projects, value, nameof(Projects));
		}

		private Subject<string> _whenReloadSuggested = new Subject<string>();
		public IObservable<string> WhenReloadSuggested => _whenReloadSuggested;

		private Subject<string> _whenSolutionChanged = new Subject<string>();
		public IObservable<string> WhenSolutionChanged => _whenSolutionChanged;

		private string _feedbackText;

		public string FeedbackText
		{
			get => _feedbackText;
			set => SetValue(ref _feedbackText, value, nameof(FeedbackText));
		}

		private string _solutionPath;

		public string SolutionPath
		{
			get => _solutionPath;
			set => SetValue(ref _solutionPath, value, nameof(SolutionPath));
		}
		
		public bool CanMoveProjectsExecute
		{
			get
			{
				if (!Projects.Any(d => d.IsSelectedForMovement))
					return false;

				var sync = ThreadHelper.JoinableTaskFactory.Run(IsSolutionFileConsistentWithRuntimeProjectsAsync);
				return sync;
			}
		}

		private ICommand _moveProjectsCommand;

		public ICommand MoveProjectsCommand
		{
			get => _moveProjectsCommand;
			set => SetValue(ref _moveProjectsCommand, value, nameof(MoveProjectsCommand));
		}

		private ICommand _toggleSelectionCommand;

		public ICommand ToggleSelectionCommand
		{
			get => _toggleSelectionCommand;
			set => SetValue(ref _toggleSelectionCommand, value, nameof(ToggleSelectionCommand));
		}

		private ICommand _saveAllChangesCommand;

		public ICommand SaveAllChangesCommand
		{
			get => _saveAllChangesCommand;
			set => SetValue(ref _saveAllChangesCommand, value, nameof(SaveAllChangesCommand));
		}

		private bool _displayError;

		public bool DisplayError
		{
			get => _displayError;
			set => SetValue(ref _displayError, value, nameof(DisplayError));
		}

		public ProjectMoverViewModel()
		{
			_disposables.Add(EventDelegator.WhenSolutionClosed
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(d => _whenSolutionChanged.OnNext(null)));

			_disposables.Add(EventDelegator.WhenSolutionOpened
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(d => _whenSolutionChanged.OnNext(d)));

			_disposables.Add(EventDelegator.WhenProjectAdded
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(d => _whenReloadSuggested.OnNext("Project added in IDE")));

			_disposables.Add(EventDelegator.WhenProjectRemoved
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(d => _whenReloadSuggested.OnNext("Project removed in IDE")));

			_disposables.Add(WhenReloadSuggested
				.Throttle(TimeSpan.FromSeconds(1))
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(OnReloadSuggested));

			_disposables.Add(WhenSolutionChanged
				.ObserveOn(Dispatcher.CurrentDispatcher)
				.Subscribe(OnSolutionChanged));
			
			MoveProjectsCommand = new RelayCommand(MoveProjectsExecute, d => CanMoveProjectsExecute);
			ToggleSelectionCommand = new RelayCommand(OnToggleExecute);
			SaveAllChangesCommand = new RelayCommand(SaveAllChangesExecute);
		}

		private async void SaveAllChangesExecute(object obj)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var solution = PackageHelper.GetDTE().Solution;
			if (!string.IsNullOrEmpty(solution.FullName))
			{
				solution.SaveAs(solution.FullName);
				_whenReloadSuggested.OnNext("User saved all files");
			}
		}

		private FileSystemWatcher _solutionFileSystemWatcher;
		private void OnSolutionChanged(string solutionPath)
		{
			try
			{
				SolutionPath = solutionPath;

				UpdateFeedback();

				_solutionFileSystemWatcher?.Dispose();

				if (string.IsNullOrEmpty(solutionPath))
					return;

				_disposables.Add(_solutionFileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(solutionPath), "*.sln"));

				var fromEventPattern = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
					add => _solutionFileSystemWatcher.Changed += add, 
					remove => _solutionFileSystemWatcher.Changed -= remove);
				_solutionFileSystemWatcher.EnableRaisingEvents = true;
				_solutionFileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

				_disposables.Add(fromEventPattern
					.Where(d => d.EventArgs.FullPath.Equals(solutionPath))
					.Subscribe(pattern => _whenReloadSuggested.OnNext("Solution file changed")));
				
				_whenReloadSuggested.OnNext("Solution has changed");
			}
			catch (Exception e)
			{
				LoggerHelper.Log(e);
			}
		}

		private void OnToggleExecute(object obj)
		{
			if (obj is ProjectMoverItemViewModel casted)
			{
				casted.IsSelectedForMovement = casted.IsSelectedForMovement;
			}

			UpdateFeedback();
		}

		private async void MoveProjectsExecute(object obj)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.ShowNewFolderButton = false;
				dialog.Description = Translations.SelectTargetDirectory;
				dialog.SelectedPath = Path.GetDirectoryName(SolutionPath);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						var moverTool = new MoverTool(Projects.Where(d => d.IsSelectedForMovement).Select(d => d.Project.AbsolutePath), SolutionPath, dialog.SelectedPath);
						await moverTool.MoveAsync();
					}
					catch (Exception e)
					{
						LoggerHelper.Log(e);
					}
				}
			}
		}

		private void OnReloadSuggested(string updateReason)
		{
			Reload(updateReason);
			this.OnPropertyChanged(nameof(CanMoveProjectsExecute));
		}

		private void Reload(string updateReason)
		{
			LoggerHelper.Log($"Reload reason: {updateReason}.");
			
			Projects.Clear();
			
			if (!string.IsNullOrEmpty(SolutionPath))
			{
				LoggerHelper.Log($"Adding projects");
				
				var solutionFile = SolutionFile.Parse(SolutionPath);

				foreach (var project in solutionFile.ProjectsInOrder.Where(d => d.ProjectType != SolutionProjectType.SolutionFolder))
				{
					var projectItem = new ProjectMoverItemViewModel(project);
					_disposables.Add(projectItem.WhenPropertyChanged.Subscribe(d => UpdateFeedback()));
					Projects.Add(projectItem);
				}
			}

			UpdateFeedback();
		}

		private void UpdateFeedback()
		{
			if (string.IsNullOrEmpty(SolutionPath))
			{
				FeedbackText = "No project loaded";
				return;
			}
			
			DisplayError = !CanMoveProjectsExecute && Projects.Any(d => d.IsSelectedForMovement);

			var selected = Projects.Count(d => d.IsSelectedForMovement);
			if (selected == 0)
			{
				FeedbackText = Translations.SelectProjectsForMoveAction;
			}
			else
			{
				FeedbackText = string.Format(Translations.MoveActionSelectedProject_0, selected);
			}
		}

		private async Task<bool> IsSolutionFileConsistentWithRuntimeProjectsAsync()
		{
			if (string.IsNullOrEmpty(SolutionPath))
				return true;

			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var solutionFile = SolutionFile.Parse(SolutionPath);
			var allProjects = SolutionHelper.GetProjectsRecursive();
			var solutionFileProjects = solutionFile.ProjectsInOrder.Where(d => d.ProjectType != SolutionProjectType.SolutionFolder).Select(d => d.AbsolutePath.ToUpperInvariant()).ToHashSet();
			var vsRuntimeProjects = allProjects.Select(d => d.FullName.ToUpperInvariant()).ToHashSet();

			return vsRuntimeProjects.All(d => solutionFileProjects.Contains(d))
				&& solutionFileProjects.All(d => vsRuntimeProjects.Contains(d));
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_disposables?.Dispose();
			_whenReloadSuggested?.Dispose();
			_whenSolutionChanged?.Dispose();
		}
	}
}