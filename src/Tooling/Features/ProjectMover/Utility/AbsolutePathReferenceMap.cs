using System.Collections.Generic;

namespace Tooling.Features.ProjectMover.Utility
{
	public class AbsolutePathReferenceMap
	{
		private readonly Dictionary<string, HashSet<string>> _values = new Dictionary<string, HashSet<string>>();

		public void AddEntries(string projectFilePath, IEnumerable<string> references)
		{
			_values.Add(projectFilePath, new HashSet<string>(references));
		}

		public IEnumerable<string> GetInversedDependencies(string path)
		{
			var processed = new HashSet<string>();

			foreach (var reference in GetInversedDependencies(path, processed))
			{
				yield return reference;
			}
		}

		private IEnumerable<string> GetInversedDependencies(string path, HashSet<string> processed)
		{
			if(!processed.Add(path))
				yield break;

			foreach (var current in _values)
			{
				if (current.Value.Contains(path))
				{
					yield return current.Key;

					foreach (var parentReference in GetInversedDependencies(current.Key, processed))
					{
						yield return parentReference;
					}
				}
			}
		}
	}
}