using System.Threading.Tasks;
using Shouldly;
using Tooling.Features.ProjectMover.Processors;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class ProjectParserTests
	{
		[Fact]
		public async Task Verify()
		{
			var processor = new ProjectFileProcessor();
			var entitiesReferences = await processor.ProcessAsync(EmbeddedTestFileUtility.GetFileStream("MoveTests.Before.entities.csproj"));
			var efReferences = await processor.ProcessAsync(EmbeddedTestFileUtility.GetFileStream("MoveTests.Before.ef.csproj"));

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