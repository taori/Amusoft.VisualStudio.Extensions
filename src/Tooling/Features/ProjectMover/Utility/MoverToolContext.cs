using System;
using System.Collections.Generic;

namespace Tooling.Features.ProjectMover.Utility
{
	public class MoverToolContext
	{
		public HashSet<string> Projects { get; }
		public string SolutionPath { get; }
		public string DestinationPath { get; }
		public MoverToolOptions Options { get; }

		public MoverToolContext(IEnumerable<string> projects, string solutionPath, string destinationPath, MoverToolOptions options)
		{
			if (projects == null)
				throw new ArgumentNullException(nameof(projects));

			Projects = new HashSet<string>(projects);
			SolutionPath = solutionPath ?? throw new ArgumentNullException(nameof(solutionPath));
			DestinationPath = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
			Options = options ?? throw new ArgumentNullException(nameof(options));
		}

	}
}