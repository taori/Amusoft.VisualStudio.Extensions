using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tooling.Features.ProjectMover
{
	public class MoverTool2Options
	{
		public IFileSystem FileSystem { get; set; } = new DefaultFileSystem();
	}
	public class MoverTool2
	{
		public IEnumerable<string> Projects { get; }
		public string SolutionPath { get; }
		public string DestinationPath { get; }
		public MoverTool2Options Options { get; }

		public MoverTool2(IEnumerable<string> projects, string solutionPath, string destinationPath, MoverTool2Options options = null)
		{
			Projects = projects ?? throw new ArgumentNullException(nameof(projects));
			SolutionPath = solutionPath ?? throw new ArgumentNullException(nameof(solutionPath));
			DestinationPath = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
			Options = options ?? new MoverTool2Options();
		}

		public async Task MoveAsync()
		{
		}
	}
}