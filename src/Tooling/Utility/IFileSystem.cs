using System.Threading.Tasks;

namespace Tooling.Utility
{
	public interface IFileSystem
	{
		Task<bool> WriteAsync(string path, string content);
		Task<string> ReadAsync(string path);
		void MoveDirectory(string source, string target);
	}
}