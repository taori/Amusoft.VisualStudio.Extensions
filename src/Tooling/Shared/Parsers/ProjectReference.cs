using System.Diagnostics;

namespace Tooling.Shared.Parsers
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