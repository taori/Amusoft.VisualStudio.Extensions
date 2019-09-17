using System.Diagnostics;

namespace Tooling.Features.ProjectMover.Utility
{
	[DebuggerDisplay("{AbsolutePath}")]
	public class MigrationInformation
	{
		public string AbsolutePath { get; set; }

		public string RelativePath { get; set; }
	}
}