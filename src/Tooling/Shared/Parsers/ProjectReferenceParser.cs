using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tooling.Shared.Parsers
{
	public class ProjectReferenceParser
	{
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