using System;
using System.IO;
using System.Threading.Tasks;
using Tooling.Utility;

namespace Tooling.Dependencies
{
	public class DefaultFileSystem : IFileSystem
	{
		/// <inheritdoc />
		public async Task<bool> WriteAsync(string path, string content)
		{
			try
			{
				using (var writer = new StreamWriter(path, false))
				{
					await writer.WriteAsync(content).ConfigureAwait(false);
				}

				return true;
			}
			catch (Exception e)
			{
				LoggerHelper.Log(e);
				return false;
			}
		}

		/// <inheritdoc />
		public async Task<string> ReadAsync(string path)
		{
			using (var streamReader = new StreamReader(path))
			{
				return await streamReader.ReadToEndAsync().ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public void MoveDirectory(string source, string target)
		{
			Directory.Move(source, target);
		}

		/// <inheritdoc />
		public void MoveFile(string source, string target)
		{
			File.Move(source, target);
		}

		/// <inheritdoc />
		public bool Exists(string combine)
		{
			return File.Exists(combine);
		}
	}
}