namespace Tooling.Features.ProjectMover.Mapping
{
	public interface IProjectPathTransformer
	{
		string RelativePath(string path);
		string AbsolutePath(string path);
	}
}