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
		public List<SolutionReference> Process(string content)
		{
			if (content == null)
				throw new ArgumentNullException(nameof(content));

			var items = new List<SolutionReference>();
			AddFromContent(items, content);
			return items;
		}

		public async Task<List<SolutionReference>> ProcessAsync(StreamReader streamReader)
		{
			if (streamReader == null)
				throw new ArgumentNullException(nameof(streamReader));

			var content = await streamReader.ReadToEndAsync();
			return Process(content);
		}

		private readonly Regex _linkReferencesExpression = new Regex("\"(?<name>[^\"]+)(?:\",\\s?)\"(?<relativePath>[^\"]+\\.(?:cs|vb)proj)\"");

		private void AddFromContent(List<SolutionReference> items, string content)
		{
			if (items == null)
				throw new ArgumentNullException(nameof(items));
			if (content == null)
				throw new ArgumentNullException(nameof(content));

			if (!_linkReferencesExpression.IsMatch(content))
				return;

			var additions = _linkReferencesExpression
				.Matches(content)
				.Cast<Match>()
				.Select(d => new SolutionReference(
					d.Groups["name"].Value, 
					d.Groups["relativePath"].Value));

			items.AddRange(additions);
		}
	}
}