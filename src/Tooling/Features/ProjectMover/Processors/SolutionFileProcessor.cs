using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tooling.Features.ProjectMover.Processors
{
	public class SolutionFileProcessor
	{
		public async Task ProcessAsync(StreamReader streamReader)
		{
			var items = new List<SolutionReference>();
			var content = await streamReader.ReadToEndAsync();
			AddFromContent(items, content);
			ProjectReferences = items;
		}

		private readonly Regex _solutionRelativeLinkReferences = new Regex("\"(?<name>[^\"]+)(?:\",\\s?)\"(?<relativePath>[^\"]+)\"");
		private void AddFromContent(List<SolutionReference> items, string content)
		{
			if (items == null)
				throw new ArgumentNullException(nameof(items));
			if (content == null)
				throw new ArgumentNullException(nameof(content));

			if (!_solutionRelativeLinkReferences.IsMatch(content))
				return;

			var additions = _solutionRelativeLinkReferences
				.Matches(content)
				.Cast<Match>()
				.Select(d => new SolutionReference(
					d.Groups["name"].Value, 
					d.Groups["relativePath"].Value));

			items.AddRange(additions);
		}

		public IReadOnlyList<SolutionReference> ProjectReferences { get; private set; }
	}
}