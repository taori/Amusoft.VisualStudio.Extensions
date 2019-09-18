using System.Threading.Tasks;
using Shouldly;
using Tooling.Features.ProjectMover.Mapping;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class ProjectPathMapperTests
	{
		[Theory]
		[InlineData(@"D:\GitHub\Project\All.sln", @"D:\GitHub\Project\Project1\Project1\project.csproj", @"D:\GitHub\Project\", @"D:\GitHub\Project\Project1\project.csproj")]
		public void SuggestedPath(string solutionPath, string projectFilePath, string targetFolder, string expected)
		{
			var mapper = new PathMapper(solutionPath);
			var result = mapper.GetSuggestedPath(projectFilePath, targetFolder);
			result.ShouldBe(expected);
		}
		[Theory]
		[InlineData(@"D:\GitHub\Project\All.sln", @"D:\GitHub\Project\Project1\Project1\project.csproj", @"D:\GitHub\Project\", @"Project1\project.csproj")]
		public void SuggestedRelativePath(string solutionPath, string projectFilePath, string targetFolder, string expected)
		{
			var mapper = new PathMapper(solutionPath);
			var result = mapper.GetSuggestedRelativePath(projectFilePath, targetFolder);
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData(@"D:\GitHub\Project\src\All.sln", @"D:\GitHub\Project\src\Project1\Project1\project.csproj", @"Project1\Project1\project.csproj")]
		[InlineData(@"D:\GitHub\Project\src\All.sln", @"D:\GitHub\Project\test\Project1\Project1\project.csproj", @"..\test\Project1\Project1\project.csproj")]
		public void RelativePath(string solutionPath, string projectFilePath, string expected)
		{
			var mapper = new PathMapper(solutionPath);
			var result = mapper.GetRelativePath(projectFilePath);
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData(@"D:\GitHub\Project\src\All.sln", @"Project1\Project1\project.csproj", @"D:\GitHub\Project\src\Project1\Project1\project.csproj")]
		[InlineData(@"D:\GitHub\Project\src\All.sln", @"..\..\..\Project1\Project1\project.csproj", @"D:\Project1\Project1\project.csproj")]
		public void AbsolutePath(string solutionPath, string relativePath, string expected)
		{
			var mapper = new PathMapper(solutionPath);
			var result = mapper.GetAbsolutePath(relativePath);
			result.ShouldBe(expected);
		}
	}
}