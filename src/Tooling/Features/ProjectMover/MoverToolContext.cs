using System.Collections.Generic;
using Microsoft.Build.Construction;
using Tooling.Features.ProjectMover.Processors;
using Tooling.Features.ProjectMover.Utility;

namespace Tooling.Features.ProjectMover
{
	public class MoverToolContext
	{
		public MoverToolContext(IEnumerable<ProjectInSolution> projectsForRewrite, string solutionPath, string targetPath)
		{
			ProjectsForRewrite = projectsForRewrite;
			SolutionPath = solutionPath;
			TargetPath = targetPath;
		}

		public IEnumerable<ProjectInSolution> ProjectsForRewrite { get; }

		public string SolutionPath { get; }
		public string TargetPath { get; }

		public List<SolutionReference> RelativeSolutionReferences { get; set; }

		public AbsolutePathReferenceMap AbsoluteSolutionReferences { get; set; }
		public Dictionary<string, string> PathSuggestions { get; set; }
	}
}