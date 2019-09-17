using System.Linq;
using Shouldly;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Utility;
using Xunit;

namespace Tooling.UnitTests
{
	public class AbsolutePathReferenceMapTests
	{
		[Fact]
		public void Case1()
		{
			var map = new AbsolutePathReferenceMap();
			map.AddEntries("root", new []{"sub1", "sub2", "sub3"});
			map.AddEntries("sub1", new []{"sub2"});

			var results = map.GetInversedDependencies("sub2").ToHashSet();

			results.Count.ShouldBe(2);
			results.ShouldContain("root");
			results.ShouldContain("sub1");
		}

		[Fact]
		public void Case2()
		{
			var map = new AbsolutePathReferenceMap();
			map.AddEntries("root", new []{"sub1", "sub2", "sub3"});
			map.AddEntries("sub1", new []{"sub2"});

			var results = map.GetInversedDependencies("root").ToHashSet();

			results.Count.ShouldBe(0);
		}

		[Fact]
		public void Case3()
		{
			var map = new AbsolutePathReferenceMap();
			map.AddEntries("root", new []{"sub1", "sub2", "sub3"});
			map.AddEntries("sub1", new []{"sub4"});
			map.AddEntries("sub2", new []{"sub4"});
			map.AddEntries("sub3", new []{"sub4"});

			var results = map.GetInversedDependencies("sub4").ToHashSet();

			results.Count.ShouldBe(4);
			results.ShouldContain("root");
			results.ShouldContain("sub1");
			results.ShouldContain("sub2");
			results.ShouldContain("sub3");
		}
	}
}