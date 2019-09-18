using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tooling.UnitTests.Utility
{
	public static class EmbeddedTestFileUtility
	{
		private static string GetManifestFilePath(string path)
		{
			var fullName = Assembly.GetAssembly(typeof(EmbeddedTestFileUtility)).FullName;
			var assemblyName = fullName.Substring(0, fullName.IndexOf(','));
			return  $"{assemblyName}.TestContent.{path}";
		}

		public static StreamReader GetFileStream(string path)
		{
			var stream = typeof(EmbeddedTestFileUtility).Assembly.GetManifestResourceStream(GetManifestFilePath(path));
			return new StreamReader(stream, Encoding.UTF8, true, 1024, true);
		}

		public static async Task<string> GetContentAsync(string path)
		{
			using (var stream = GetFileStream(path))
			{
				return await stream.ReadToEndAsync().ConfigureAwait(false);
			}
		}

		public static async Task<T> GetJsonAsync<T>(string path)
		{
			using (var reader = GetFileStream(path))
			{
				return JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync().ConfigureAwait(false));
			}
		}
	}
}