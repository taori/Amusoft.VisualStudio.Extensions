using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Processors;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class FileSystemMock : IFileSystem
	{
		Dictionary<string, string> _values = new Dictionary<string, string>();
		public async Task RegisterContent(string path, Func<StreamReader> streamAccess)
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
	public class MoverToolTests
	{
		[Fact]
		public async Task Verify()
		{
			var projects = new[]
			{
				@"D:\GitHub\Projects\MyProject\Some\Unnecessary\Folders\Project1.EF.csproj",
				@"D:\GitHub\Projects\MyProject\Some\Unnecessary\Folders\Project1.Entities.csproj",
			};
			var options = new MoverTool2Options();
			var fileSystemMock = new FileSystemMock();
			fileSystemMock.RegisterContent(@"D:\GitHub\Projects\MyProject\Some\Unnecessary\Folders\Project1.EF.csproj", () => EmbeddedTestFileUtility.GetFileStream("case1.ef.original.sln"));
			fileSystemMock.RegisterContent(@"D:\GitHub\Projects\MyProject\Some\Unnecessary\Folders\Project1.Entities.csproj", () => EmbeddedTestFileUtility.GetFileStream("case1.entities.original.sln"));
			options.FileSystem = fileSystemMock;
			var moverTool = new MoverTool2(projects, @"D:\GitHub\Projects\MyProject\All.sln", @"D:\GitHub\Projects\MyProject\", options);
			await moverTool.MoveAsync();


//			;
		}
	}
}