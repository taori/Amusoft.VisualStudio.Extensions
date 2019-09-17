using System.Diagnostics;

namespace Tooling.Features.ProjectMover.Processors
{
	[DebuggerDisplay("{RelativePath}")]
	public class ProjectReference
	{
		/// <inheritdoc />
		public ProjectReference(string relativePath)
		{
			RelativePath = relativePath;
		}

		public string RelativePath { get; set; }
	}
}