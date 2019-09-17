using System.Collections.Generic;
using Microsoft.Build.Construction;
using Tooling.Features.ProjectMover.Processors;
using Tooling.Features.ProjectMover.Utility;

namespace Tooling.Features.ProjectMover
{
	public class MoverToolContext
	{
		public MoverToolContext(IEnumerable<string> rewriteTargets, string solutionPath, string targetPath)
		{
			RewriteTargets = rewriteTargets;
			SolutionPath = solutionPath;
			TargetPath = targetPath;
		}

		/// <summary>
		/// Projects which are scheduled to be moved
		/// </summary>
		public IEnumerable<string> RewriteTargets { get; }

		/// <summary>
		/// Full path to solution file
		/// </summary>
		public string SolutionPath { get; }

		/// <summary>
		/// Selected folder for RewriteTargets
		/// </summary>
		public string TargetPath { get; }

		public List<SolutionReference> RelativeSolutionReferences { get; set; }

		public AbsolutePathReferenceMap AbsoluteSolutionReferences { get; set; }
		public Dictionary<string, string> PathSuggestions { get; set; }
	}
}