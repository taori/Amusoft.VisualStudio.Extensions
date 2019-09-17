using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tooling.Features.ProjectMover.Processors
{
	public class ProjectFileProcessor
	{
		public async Task<List<ProjectReference>> ProcessAsync(StreamReader streamReader)
		{
			if (streamReader == null)
				throw new ArgumentNullException(nameof(streamReader));

			var content = await streamReader.ReadToEndAsync();

			return Process(content);
		}
		public List<ProjectReference> Process(string content)
		{
			if (content == null)
				throw new ArgumentNullException(nameof(content));

			var items = new List<ProjectReference>();
			AddFromContent(items, content);

			return items;
		}

		private readonly Regex _linkReferencesExpression = new Regex("Include=\"(?<relativePath>[^\"]+\\.(csproj|vbproj))\"");

		private void AddFromContent(List<ProjectReference> items, string content)
		{
			if (!_linkReferencesExpression.IsMatch(content))
				return;

			var matches = _linkReferencesExpression
				.Matches(content)
				.Cast<Match>()
				.Select(d => d.Groups["relativePath"].Value)
				.Select(d => new ProjectReference(d));

			items.AddRange(matches);
		}
	}
}