using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Processors;
using Tooling.UnitTests.Mocks;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class MoverToolTests
	{
		[Fact]
		public async Task MoveMultiple()
		{
			var projects = new[]
			{
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj",
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj",
			};
			var afterProjects = new[]
			{
				@"D:\GitHub\Projects\MyProject\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj",
				@"D:\GitHub\Projects\MyProject\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj",
			};
			var options = new MoverTool2Options();
			var fileSystemMock = new FileSystemMock();
			await fileSystemMock.RegisterContentAsync(projects[0], () => EmbeddedTestFileUtility.GetFileStream("case1.entities.original.csproj"));
			await fileSystemMock.RegisterContentAsync(projects[1], () => EmbeddedTestFileUtility.GetFileStream("case1.ef.original.csproj"));
			await fileSystemMock.RegisterContentAsync(@"D:\GitHub\Projects\MyProject\All.sln", () => EmbeddedTestFileUtility.GetFileStream("case1.original.sln"));
			
			options.FileSystem = fileSystemMock;
			var moverTool = new MoverTool2(projects, @"D:\GitHub\Projects\MyProject\All.sln", @"D:\GitHub\Projects\MyProject\", options);
			await moverTool.MoveAsync();

			moverTool.SolutionReferences.Count.ShouldBe(2);
			var solutionReference0 = moverTool.SolutionReferences.FirstOrDefault(d => d.Before.AbsolutePath == projects[0]);
			solutionReference0.ShouldNotBeNull();
			solutionReference0.Before.AbsolutePath.ShouldBe(projects[0]);
			solutionReference0.Before.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
			solutionReference0.After.AbsolutePath.ShouldBe(afterProjects[0]);
			solutionReference0.After.RelativePath.ShouldBe(@"EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");

			var solutionReference1 = moverTool.SolutionReferences.FirstOrDefault(d => d.Before.AbsolutePath == projects[1]);
			solutionReference1.ShouldNotBeNull();
			solutionReference1.Before.AbsolutePath.ShouldBe(projects[1]);
			solutionReference1.Before.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj");
			solutionReference1.After.AbsolutePath.ShouldBe(afterProjects[1]);
			solutionReference1.After.RelativePath.ShouldBe(@"EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj");

			var projectReference0 = moverTool.ProjectReferences.FirstOrDefault(d => d.Key == projects[0]);
			projectReference0.ShouldNotBeNull();
			projectReference0.Value.Count.ShouldBe(0);

			var projectReference1 = moverTool.ProjectReferences.FirstOrDefault(d => d.Key == projects[1]);
			projectReference1.ShouldNotBeNull();
			projectReference1.Value.Count.ShouldBe(1);
			projectReference1.Value[0].Before.AbsolutePath.ShouldBe(solutionReference0.Before.AbsolutePath);
			projectReference1.Value[0].Before.RelativePath.ShouldBe(@"..\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
			projectReference1.Value[0].After.AbsolutePath.ShouldBe(solutionReference0.After.AbsolutePath);
			projectReference1.Value[0].After.RelativePath.ShouldBe(@"..\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
		}
	}
}