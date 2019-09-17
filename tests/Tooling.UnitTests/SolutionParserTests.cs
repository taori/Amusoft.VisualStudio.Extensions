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
		public async Task Case1Integrity()
		{
			var processor = new SolutionFileProcessor();
			var references = await processor.ProcessAsync(EmbeddedTestFileUtility.GetFileStream("case1.original.sln"));

			references.Count.ShouldBe(2);

			var expected = new[]
			{
				new SolutionReference("EF.Attempt1.EntityFramework", @"EF.Attempt1\EF.Attempt1.EntityFramework\EF.Attempt1.EntityFramework.csproj"),
				new SolutionReference("EF.Attempt1.Entities", @"EF.Attempt1\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj"),
			};

			for (int i = 0; i < expected.Length; i++)
			{
				references.ShouldContain(expected[i]);
			}
		}
	}
}
