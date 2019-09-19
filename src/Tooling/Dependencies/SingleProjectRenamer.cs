using System;
using System.IO;
using Tooling.Features.ProjectMover;
using Tooling.Features.ProjectMover.Mapping;

namespace Tooling.Dependencies
{
	public class SingleProjectRenamer : IProjectPathTransformer
	{
		public string PreviousName { get; }
		public string NewName { get; }

		public SingleProjectRenamer(string previousName, string newName)
		{
			PreviousName = previousName ?? throw new ArgumentNullException(nameof(previousName));
			NewName = newName ?? throw new ArgumentNullException(nameof(newName));
		}

		/// <inheritdoc />
		public string RelativePath(string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			var extension = Path.GetExtension(path);
			if (path.LastIndexOf($@"{PreviousName}\{PreviousName}{extension}", StringComparison.OrdinalIgnoreCase) is var index && index >= 0)
			{
				return path.Substring(0, index) + $@"{NewName}\{NewName}{extension}";
			}

			return path;
		}

		/// <inheritdoc />
		public string AbsolutePath(string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			var extension = Path.GetExtension(path);
			if (path.LastIndexOf($@"{PreviousName}\{PreviousName}{extension}", StringComparison.OrdinalIgnoreCase) is var index && index >= 0)
			{
				return path.Substring(0, index) + $@"{NewName}\{NewName}{extension}";
			}

			return path;
		}
	}
}