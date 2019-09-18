using System;
using System.Threading.Tasks;
using Shouldly;
using Tooling.Shared.Parsers;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class SolutionParserTests
	{
		[Fact]
		public async Task Case1Integrity()
		{
			var processor = new SolutionReferenceParser();
			var references = processor.Process(await EmbeddedTestFileUtility.GetFileStream("MoveTests.Before.solution.sln").ReadToEndAsync());

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
