using System.Diagnostics;

namespace Tooling.Features.ProjectMover.Utility
{
	[DebuggerDisplay("{Before} -> {After}")]
	public class HistoryInformation
	{
		public MigrationInformation Before { get; set; } = new MigrationInformation();

		public MigrationInformation After { get; set; } = new MigrationInformation();
	}
}