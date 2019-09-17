using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tooling.Features.ProjectMover;

namespace Tooling.UnitTests.Mocks
{
	public class FileSystemMock : IFileSystem
	{
		Dictionary<string, string> _values = new Dictionary<string, string>();
		public async Task RegisterContentAsync(string path, Func<StreamReader> streamAccess)
		{
			var streamReader = streamAccess();
			await WriteAsync(path, await streamReader.ReadToEndAsync());
		}

		/// <inheritdoc />
		public async Task<bool> WriteAsync(string path, string content)
		{
			if (_values.ContainsKey(path))
			{
				_values[path] = content;
			}
			else
			{
				_values.Add(path, content);
			}

			return true;
		}

		/// <inheritdoc />
		public async Task<string> ReadAsync(string path)
		{
			return _values.TryGetValue(path, out var value) ? value : throw new Exception("No content available");
		}
	}
}