using System.Collections.Generic;
using System.Diagnostics;

namespace Tooling.Features.ProjectMover.Processors
{
	[DebuggerDisplay("{Name}")]
	public class SolutionReference 
	{
		private sealed class NameRelativeNameEqualityComparer : IEqualityComparer<SolutionReference>
		{
			public bool Equals(SolutionReference x, SolutionReference y)
			{
				if (ReferenceEquals(x, y))
					return true;
				if (ReferenceEquals(x, null))
					return false;
				if (ReferenceEquals(y, null))
					return false;
				if (x.GetType() != y.GetType())
					return false;

				return x.Name == y.Name && x.RelativeName == y.RelativeName;
			}

			public int GetHashCode(SolutionReference obj)
			{
				unchecked
				{
					return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ (obj.RelativeName != null ? obj.RelativeName.GetHashCode() : 0);
				}
			}
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return NameRelativeNameComparer.Equals(this, obj as SolutionReference);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return NameRelativeNameComparer.GetHashCode(this);
		}

		public static IEqualityComparer<SolutionReference> NameRelativeNameComparer { get; } = new NameRelativeNameEqualityComparer();

		/// <inheritdoc />
		public SolutionReference(string name, string relativeName)
		{
			Name = name;
			RelativeName = relativeName;
		}

		public string Name { get; set; }

		public string RelativeName { get; set; }
	}
}