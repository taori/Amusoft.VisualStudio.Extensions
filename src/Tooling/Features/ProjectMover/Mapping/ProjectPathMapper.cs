using System;
using System.IO;
using System.Linq;

namespace Tooling.Features.ProjectMover.Mapping
{
	public class ProjectPathMapper
	{
		public Uri ReferencePath { get; set; }

		/// <inheritdoc />
		public ProjectPathMapper(string referencePath)
		{
			if (referencePath == null)
				throw new ArgumentNullException(nameof(referencePath));

			ReferencePath = new Uri(referencePath, UriKind.Absolute);
		}

		public string GetSuggestedPath(string projectFilePath, string targetFolder)
		{
			if (projectFilePath == null)
				throw new ArgumentNullException(nameof(projectFilePath));
			if (targetFolder == null)
				throw new ArgumentNullException(nameof(targetFolder));

			var fileName = Path.GetFileName(projectFilePath);
			var directoryName = Path.GetDirectoryName(projectFilePath).Split(new[] {Path.DirectorySeparatorChar}).Last();
			var desiredName = Path.Combine(targetFolder, directoryName, fileName);

			return desiredName;
		}

		public string GetSuggestedRelativePath(string projectFilePath, string targetFolder)
		{
			if (projectFilePath == null)
				throw new ArgumentNullException(nameof(projectFilePath));
			if (targetFolder == null)
				throw new ArgumentNullException(nameof(targetFolder));

			var suggestion = GetSuggestedPath(projectFilePath, targetFolder);
			var suggestionPath = new Uri(suggestion, UriKind.Absolute);
			var relativePath = ReferencePath.MakeRelativeUri(suggestionPath);

			return relativePath.OriginalString.Replace('/', Path.DirectorySeparatorChar);
		}

		public string GetRelativePath(string projectFilePath)
		{
			if (projectFilePath == null)
				throw new ArgumentNullException(nameof(projectFilePath));

			var projectAbsolute = new Uri(projectFilePath, UriKind.Absolute);
			var relative = ReferencePath.MakeRelativeUri(projectAbsolute);

			return relative.OriginalString.Replace('/', Path.DirectorySeparatorChar);
		}
	}
}