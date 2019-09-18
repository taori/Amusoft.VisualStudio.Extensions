using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Tooling.Dependencies;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Utility;
using Tooling.UnitTests.Mocks;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class ProjectRenameFeatureTests
	{
		[Fact]
		public async Task MoveDependency()
		{
			var projects = new[]
			{
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj",
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj",
			};
			var afterProjects = new[]
			{
				@"D:\GitHub\Projects\MyProject\Apples.Bananas\Apples.Bananas.csproj",
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj",
			};
			var options = new MoverToolOptions();
			options.ProjectPathTransformer = new SingleProjectRenamer("EF.Attempt1.Entities", "Apples.Bananas");
			var fileSystemMock = new FileSystemMock();
			var slnPath = @"D:\GitHub\Projects\MyProject\All.sln";
			await fileSystemMock.RegisterContentAsync(projects[0], () => EmbeddedTestFileUtility.GetFileStream("ProjectRenameTests.Before.entities.csproj"));
			await fileSystemMock.RegisterContentAsync(projects[1], () => EmbeddedTestFileUtility.GetFileStream("ProjectRenameTests.Before.ef.csproj"));
			await fileSystemMock.RegisterContentAsync(slnPath, () => EmbeddedTestFileUtility.GetFileStream("ProjectRenameTests.Before.solution.sln"));

			options.FileSystem = fileSystemMock;
			var moverTool = new MoverTool(new []{ projects[0] }, slnPath, @"D:\GitHub\Projects\MyProject\", options);
			await moverTool.MoveAsync();

			moverTool.SolutionReferences.Count.ShouldBe(2);
			var solutionReference0 = moverTool.SolutionReferences.FirstOrDefault(d => d.Before.AbsolutePath == projects[0]);
			solutionReference0.ShouldNotBeNull();
			solutionReference0.Before.AbsolutePath.ShouldBe(projects[0]);
			solutionReference0.Before.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
			solutionReference0.After.AbsolutePath.ShouldBe(afterProjects[0]);
			solutionReference0.After.RelativePath.ShouldBe(@"Apples.Bananas\Apples.Bananas.csproj");

			var solutionReference1 = moverTool.SolutionReferences.FirstOrDefault(d => d.Before.AbsolutePath == projects[1]);
			solutionReference1.ShouldNotBeNull();
			solutionReference1.Before.AbsolutePath.ShouldBe(projects[1]);
			solutionReference1.Before.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj");
			solutionReference1.After.AbsolutePath.ShouldBe(afterProjects[1]);
			solutionReference1.After.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj");

			var projectReference0 = moverTool.ProjectReferences.FirstOrDefault(d => d.Key == projects[0]);
			projectReference0.ShouldNotBeNull();
			projectReference0.Value.Count.ShouldBe(0);

			var projectReference1 = moverTool.ProjectReferences.FirstOrDefault(d => d.Key == projects[1]);
			projectReference1.ShouldNotBeNull();
			projectReference1.Value.Count.ShouldBe(1);
			projectReference1.Value[0].Before.AbsolutePath.ShouldBe(solutionReference0.Before.AbsolutePath);
			projectReference1.Value[0].Before.RelativePath.ShouldBe(@"..\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
			projectReference1.Value[0].After.AbsolutePath.ShouldBe(solutionReference0.After.AbsolutePath);
			projectReference1.Value[0].After.RelativePath.ShouldBe(@"..\..\Apples.Bananas\Apples.Bananas.csproj");

			(await fileSystemMock.ReadAsync(slnPath))
				.ShouldBe(await EmbeddedTestFileUtility.GetContentAsync("ProjectRenameTests.MoveDependency.solution.sln"));

			(await fileSystemMock.ReadAsync(@"D:\GitHub\Projects\MyProject\Apples.Bananas\Apples.Bananas.csproj"))
				.ShouldBe(await EmbeddedTestFileUtility.GetContentAsync("ProjectRenameTests.MoveDependency.entities.csproj"));

			(await fileSystemMock.ReadAsync(@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj"))
				.ShouldBe(await EmbeddedTestFileUtility.GetContentAsync("ProjectRenameTests.MoveDependency.ef.csproj"));
		}

		[Fact]
		public async Task MoveNonDependency()
		{
			var projects = new[]
			{
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj",
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj",
			};
			var afterProjects = new[]
			{
				@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj",
				@"D:\GitHub\Projects\MyProject\Apples.Bananas\Apples.Bananas.csproj",
			};
			var options = new MoverToolOptions();
			options.ProjectPathTransformer = new SingleProjectRenamer("EF.Attempt1.EntityFramework", "Apples.Bananas");
			var fileSystemMock = new FileSystemMock();
			var slnPath = @"D:\GitHub\Projects\MyProject\All.sln";
			await fileSystemMock.RegisterContentAsync(projects[0], () => EmbeddedTestFileUtility.GetFileStream("ProjectRenameTests.Before.entities.csproj"));
			await fileSystemMock.RegisterContentAsync(projects[1], () => EmbeddedTestFileUtility.GetFileStream("ProjectRenameTests.Before.ef.csproj"));
			await fileSystemMock.RegisterContentAsync(slnPath, () => EmbeddedTestFileUtility.GetFileStream("ProjectRenameTests.Before.solution.sln"));

			options.FileSystem = fileSystemMock;
			var moverTool = new MoverTool(new[] { projects[1] }, slnPath, @"D:\GitHub\Projects\MyProject\", options);
			await moverTool.MoveAsync();

			moverTool.SolutionReferences.Count.ShouldBe(2);
			var solutionReference0 = moverTool.SolutionReferences.FirstOrDefault(d => d.Before.AbsolutePath == projects[0]);
			solutionReference0.ShouldNotBeNull();
			solutionReference0.Before.AbsolutePath.ShouldBe(projects[0]);
			solutionReference0.Before.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
			solutionReference0.After.AbsolutePath.ShouldBe(afterProjects[0]);
			solutionReference0.After.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");

			var solutionReference1 = moverTool.SolutionReferences.FirstOrDefault(d => d.Before.AbsolutePath == projects[1]);
			solutionReference1.ShouldNotBeNull();
			solutionReference1.Before.AbsolutePath.ShouldBe(projects[1]);
			solutionReference1.Before.RelativePath.ShouldBe(@"EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj");
			solutionReference1.After.AbsolutePath.ShouldBe(afterProjects[1]);
			solutionReference1.After.RelativePath.ShouldBe(@"Apples.Bananas\Apples.Bananas.csproj");

			var projectReference0 = moverTool.ProjectReferences.FirstOrDefault(d => d.Key == projects[0]);
			projectReference0.ShouldNotBeNull();
			projectReference0.Value.Count.ShouldBe(0);

			var projectReference1 = moverTool.ProjectReferences.FirstOrDefault(d => d.Key == projects[1]);
			projectReference1.ShouldNotBeNull();
			projectReference1.Value.Count.ShouldBe(1);
			projectReference1.Value[0].Before.AbsolutePath.ShouldBe(solutionReference0.Before.AbsolutePath);
			projectReference1.Value[0].Before.RelativePath.ShouldBe(@"..\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");
			projectReference1.Value[0].After.AbsolutePath.ShouldBe(solutionReference0.After.AbsolutePath);
			projectReference1.Value[0].After.RelativePath.ShouldBe(@"..\EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj");

			(await fileSystemMock.ReadAsync(slnPath))
				.ShouldBe(await EmbeddedTestFileUtility.GetContentAsync("ProjectRenameTests.MoveNonDependency.solution.sln"));

			(await fileSystemMock.ReadAsync(@"D:\GitHub\Projects\MyProject\EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj"))
				.ShouldBe(await EmbeddedTestFileUtility.GetContentAsync("ProjectRenameTests.MoveNonDependency.entities.csproj"));

			(await fileSystemMock.ReadAsync(@"D:\GitHub\Projects\MyProject\Apples.Bananas\Apples.Bananas.csproj"))
				.ShouldBe(await EmbeddedTestFileUtility.GetContentAsync("ProjectRenameTests.MoveNonDependency.ef.csproj"));
		}
	}
}