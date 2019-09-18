using Microsoft.Build.Construction;
using Tooling.Shared;

namespace Tooling.Features.ProjectMover.ViewModels
{
	public class ProjectMoverItemViewModel : ViewModelBase
	{
		public ProjectInSolution Project { get; }

		public ProjectMoverItemViewModel(ProjectInSolution project)
		{
			Project = project;
			RelativePath = project.RelativePath;
		}

		public string RelativePath { get; set; }

		private bool _isSelectedForMovement;

		public bool IsSelectedForMovement
		{
			get => _isSelectedForMovement;
			set => SetValue(ref _isSelectedForMovement, value, nameof(IsSelectedForMovement));
		}
	}
}