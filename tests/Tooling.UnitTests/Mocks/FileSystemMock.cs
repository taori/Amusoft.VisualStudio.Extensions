using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling.Features.ProjectMover;
using Tooling.Utility;

namespace Tooling.UnitTests.Mocks
{
	public class FileSystemMock : IFileSystem
	{
		Dictionary<string, string> _values = new Dictionary<string, string>();
		public async Task RegisterContentAsync(string path, Func<StreamReader> streamAccess)
		{
			var streamReader = streamAccess();
			await WriteAsync(path, await streamReader.ReadToEndAsync(), Encoding.UTF8);
		}

		/// <inheritdoc />
		public async Task<bool> WriteAsync(string path, string content, Encoding encoding)
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

		/// <inheritdoc />
		public void MoveDirectory(string source, string target)
		{
			var sourceUpper = source.ToUpperInvariant();
			var items = _values.GroupBy(d => Path.GetDirectoryName(d.Key).ToUpperInvariant());
			foreach (var directoryGroup in items)
			{
				if (directoryGroup.Key == sourceUpper)
				{
					foreach (var keyValuePair in directoryGroup)
					{
						var newName = Path.Combine(target, Path.GetFileName(keyValuePair.Key));
						if (_values.Remove(keyValuePair.Key))
						{
							if (_values.ContainsKey(newName))
							{
								_values[newName] = keyValuePair.Value;
							}
							else
							{
								_values.Add(newName, keyValuePair.Value);
							}
						}
					}
				}
			}
		}

		/// <inheritdoc />
		public void MoveFile(string source, string target)
		{
			if (_values.TryGetValue(source, out var value))
			{
				if (_values.ContainsKey(target))
				{
					_values[target] = value;
				}
				else
				{
					_values.Add(target, value);
				}
			}
			else
			{
				throw new FileNotFoundException($"{source}");
			}
		}

		/// <inheritdoc />
		public bool Exists(string combine)
		{
			return _values.ContainsKey(combine);
		}
	}
}