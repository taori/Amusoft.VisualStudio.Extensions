using System;
using System.Threading.Tasks;
using Shouldly;
using Tooling.Features.ProjectMover.Processors;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class SolutionParserTests
	{
		[Fact]
		public async Task VerifyProcessing()
		{
			var processor = new SolutionFileProcessor();
			await processor.ProcessAsync(EmbeddedTestFileUtility.GetFileStream("ExtensionTests.sln"));
			var references = processor.ProjectReferences;

			references.Count.ShouldBe(14);

			var expected = new[]
			{
				new SolutionReference("EF.Attempt1.EntityFramework", @"EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj"),
				new SolutionReference("EF.Attempt1.Entities", @"EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj"),
				new SolutionReference("EF.Attempt2.EntityFramework", @"EF.Attempt2\EF.Attempt2.EntityFramework\EF.Attempt2.EntityFramework.csproj"),
				new SolutionReference("EF.Attempt2.Entities", @"EF.Attempt2\EF.Attempt2.Entities\EF.Attempt2.Entities.csproj"),
				new SolutionReference("EF.Attempt3.EntityFramework", @"EF.Attempt3\EF.Attempt3.EntityFramework\EF.Attempt3.EntityFramework.csproj"),
				new SolutionReference("EF.Attempt3.Entities", @"EF.Attempt3\EF.Attempt3.Entities\EF.Attempt3.Entities.csproj"),
				new SolutionReference("EF.Attempt4.EntityFramework", @"EF.Attempt4\EF.Attempt4.EntityFramework\EF.Attempt4.EntityFramework.csproj"),
				new SolutionReference("EF.Attempt4.Entities", @"EF.Attempt4\EF.Attempt4.Entities\EF.Attempt4.Entities.csproj"),
				new SolutionReference("EF.Attempt5.EntityFramework", @"EF.Attempt5\EF.Attempt5.EntityFramework\EF.Attempt5.EntityFramework.csproj"),
				new SolutionReference("EF.Attempt5.Entities", @"EF.Attempt5\EF.Attempt5.Entities\EF.Attempt5.Entities.csproj"),

				new SolutionReference("VS.VSIXProject", @"VS.VSIXProject\VS.VSIXProject.csproj"),
				new SolutionReference("VS.VsixAnalyzer", @"VS.VsixAnalyzer\VS.VsixAnalyzer\VS.VsixAnalyzer.csproj"),
				new SolutionReference("VS.VsixAnalyzer.Test", @"VS.VsixAnalyzer\VS.VsixAnalyzer.Test\VS.VsixAnalyzer.Test.csproj"),
				new SolutionReference("VS.VsixAnalyzer.Vsix", @"VS.VsixAnalyzer\VS.VsixAnalyzer.Vsix\VS.VsixAnalyzer.Vsix.csproj"),
			};

			for (int i = 0; i < expected.Length; i++)
			{
				references.ShouldContain(expected[i]);
			}
		}
	}
}
