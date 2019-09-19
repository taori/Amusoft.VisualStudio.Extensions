using Tooling.Features.ProjectMover.Mapping;

namespace Tooling.Features.ProjectMover.Utility
{
	public class NoopPathTransformer : IProjectPathTransformer
	{
		/// <inheritdoc />
		public string RelativePath(string path)
		{
			return path;
		}

		/// <inheritdoc />
		public string AbsolutePath(string path)
		{
			return path;
		}
	}
}