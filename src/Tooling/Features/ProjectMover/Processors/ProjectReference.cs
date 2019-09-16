namespace Tooling.Features.ProjectMover.Processors
{
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