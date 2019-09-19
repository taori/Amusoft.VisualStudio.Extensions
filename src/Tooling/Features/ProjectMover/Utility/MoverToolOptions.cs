using Tooling.Dependencies;
using Tooling.Features.ProjectMover.Mapping;
using Tooling.Utility;

namespace Tooling.Features.ProjectMover.Utility
{
	public class MoverToolOptions
	{
		public IFileSystem FileSystem { get; set; } = new DefaultFileSystem();

		public IProjectPathTransformer ProjectPathTransformer { get; set; } = new NoopPathTransformer();
	}
}