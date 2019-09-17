using System.Threading.Tasks;
using Shouldly;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Processors;
using Tooling.UnitTests.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class MoverToolTests
	{
		[Fact]
		public async Task Verify()
		{
			//			var processor = MoverTool.MoveAsync();
			EmbeddedTestFileUtility.GetFileStream("case1.original.sln");
		}
	}
}