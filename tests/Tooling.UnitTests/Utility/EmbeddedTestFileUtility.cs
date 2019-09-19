using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tooling.UnitTests.Utility
{
	public static class EmbeddedTestFileUtility
	{
		private static string GetManifestPath()
		{
			var fullName = Assembly.GetAssembly(typeof(EmbeddedTestFileUtility)).FullName;
			var assemblyName = fullName.Substring(0, fullName.IndexOf(','));
			return $"{assemblyName}.TestContent.";
		}

		private static string GetManifestFilePath(string path)
		{
			return  $"{GetManifestPath()}{path}";
		}

		public static IEnumerable<string> GetStreamsStartingWith(string path)
		{
			var names = typeof(EmbeddedTestFileUtility).Assembly.GetManifestResourceNames().Where(d => d.StartsWith(GetManifestPath() + path));
			return names;
		}

		public static StreamReader GetFileStream(string path, bool fullPath = false)
		{
			if (!fullPath)
				path = GetManifestFilePath(path);
			var stream = typeof(EmbeddedTestFileUtility).Assembly.GetManifestResourceStream(path);

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