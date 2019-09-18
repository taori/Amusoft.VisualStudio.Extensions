using System.Threading.Tasks;
using Shouldly;
using Tooling.Shared.Parsers;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class ProjectParserTests
	{
		[Fact]
		public async Task Verify()
		{
			var processor = new ProjectReferenceParser();
			var entitiesReferences = processor.Process(await EmbeddedTestFileUtility.GetFileStream("MoveTests.Before.entities.csproj").ReadToEndAsync());
			var efReferences = processor.Process( await EmbeddedTestFileUtility.GetFileStream("MoveTests.Before.ef.csproj").ReadToEndAsync());

			entitiesReferences.Count.ShouldBe(0);

			efReferences.Count.ShouldBe(1);

			var expectedEfReferences = new[]
			{
				new ProjectReference(@"..\EF.Attempt1.Entities\EF.Attempt1.Entities.csproj"),
			};

			for (int i = 0; i < expectedEfReferences.Length; i++)
			{
				expectedEfReferences.ShouldContain(expectedEfReferences[i]);
			}
		}
	}
}