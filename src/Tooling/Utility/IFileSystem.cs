using System.Text;
using System.Threading.Tasks;

namespace Tooling.Utility
{
	public interface IFileSystem
	{
		Task<bool> WriteAsync(string path, string content, Encoding encoding);
		Task<string> ReadAsync(string path);
		void MoveDirectory(string source, string target);
		void MoveFile(string source, string target);
		bool Exists(string combine);
	}
}