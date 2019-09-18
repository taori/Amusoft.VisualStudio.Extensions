using Tooling.Dependencies;
using Tooling.Utility;

namespace Tooling.Features.ProjectMover.Utility
{
	public class MoverToolOptions
	{
		public IFileSystem FileSystem { get; set; } = new DefaultFileSystem();

		public IProjectPathTransformer ProjectPathTransformer { get; set; } = new NoopPathTransformer();
	}
}