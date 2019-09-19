using System;
using System.Collections.Generic;
using System.Linq;

namespace Tooling.UnitTests.Utility
{
	public static class EmbeddedTestFileGrouper
	{
		private static IEnumerable<(string start, string end)> EmbeddedEntries(string groupName)
		{
			var items = EmbeddedTestFileUtility
				.GetStreamsStartingWith(groupName)
				.Select(d => new
				{
					startSection = d.Substring(0, d.IndexOf(groupName) + groupName.Length),
					endSection = d.Substring(d.IndexOf(groupName) + groupName.Length)
				});

			foreach (var item in items)
			{
				yield return (item.startSection, item.endSection);
			}
		}

		public static IEnumerable<(string before, string after)> Group(string embeddedGroupOne, string embeddedGroupTwo)
		{
			var grouped = EmbeddedEntries(embeddedGroupOne)
				.Concat(EmbeddedEntries(embeddedGroupTwo))
				.GroupBy(d => d.end);

			foreach (var item in grouped.OrderBy(d => d.Key))
			{
				if(item.Count() != 2)
					throw new Exception($"There is no mirror pair for {item.Key}");

				var pairs = item.ToArray();

				yield return ($"{pairs[0].start}{pairs[0].end}", $"{pairs[1].start}{pairs[1].end}");
			}
		}
	}
}