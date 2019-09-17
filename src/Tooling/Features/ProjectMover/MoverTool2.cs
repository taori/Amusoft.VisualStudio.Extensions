using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tooling.Features.ProjectMover.Mapping;
using Tooling.Features.ProjectMover.Processors;

namespace Tooling.Features.ProjectMover
{
	public class MoverTool2Options
	{
		public IFileSystem FileSystem { get; set; } = new DefaultFileSystem();
	}

	public class MoverTool2Context
	{
		public HashSet<string> Projects { get; }
		public string SolutionPath { get; }
		public string DestinationPath { get; }
		public MoverTool2Options Options { get; }

		public MoverTool2Context(IEnumerable<string> projects, string solutionPath, string destinationPath, MoverTool2Options options)
		{
			if (projects == null)
				throw new ArgumentNullException(nameof(projects));

			Projects = new HashSet<string>(projects);
			SolutionPath = solutionPath ?? throw new ArgumentNullException(nameof(solutionPath));
			DestinationPath = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
			Options = options ?? throw new ArgumentNullException(nameof(options));
		}

	}

	[DebuggerDisplay("{AbsolutePath}")]
	public class MigrationInformation
	{
		public string AbsolutePath { get; set; }

		public string RelativePath { get; set; }
	}

	[DebuggerDisplay("{Before} -> {After}")]
	public class HistoryInformation
	{
		public MigrationInformation Before { get; set; } = new MigrationInformation();

		public MigrationInformation After { get; set; } = new MigrationInformation();
	}

	public class MoverTool2
	{
		public MoverTool2(IEnumerable<string> projects, string solutionPath, string destinationPath, MoverTool2Options options = null)
		{
			Context = new MoverTool2Context(projects, solutionPath, destinationPath, options ?? new MoverTool2Options());
		}

		public MoverTool2Context Context { get; set; }

		public List<HistoryInformation> SolutionReferences { get; set; } = new List<HistoryInformation>();

		public Dictionary<string, List<HistoryInformation>> ProjectReferences { get; set; } = new Dictionary<string, List<HistoryInformation>>();
		
		public async Task MoveAsync()
		{
			await CollectInformationAsync();
		}

		private async Task CollectInformationAsync()
		{
			await CollectSolutionInformationAsync();
			await CollectProjectsInformationAsync();
		}

		private async Task CollectProjectsInformationAsync()
		{
			foreach (var solutionReference in SolutionReferences)
			{
				var projectProcessor = new ProjectFileProcessor();
				var projectReferences = projectProcessor.Process(await Context.Options.FileSystem.ReadAsync(solutionReference.Before.AbsolutePath));
				var historyInformations = new List<HistoryInformation>();

				foreach (var projectReference in projectReferences)
				{
					var referenceHistory = new HistoryInformation();
//					referenceHistory.Before.RelativePath = projectReference.RelativePath;
					referenceHistory.Before.RelativePath = "4256";
					referenceHistory.Before.AbsolutePath = new PathMapper(solutionReference.Before.AbsolutePath).GetAbsolutePath(projectReference.RelativePath);

					if (Context.Projects.Contains(referenceHistory.Before.AbsolutePath))
					{
						var targetReference = SolutionReferences.FirstOrDefault(d => string.Equals(d.Before.AbsolutePath, referenceHistory.Before.AbsolutePath, StringComparison.OrdinalIgnoreCase));
						if(targetReference == null)
							throw new Exception("Target reference for path not found");

						referenceHistory.After.AbsolutePath = targetReference.After.AbsolutePath;
						referenceHistory.After.RelativePath = new PathMapper(solutionReference.After.AbsolutePath).GetRelativePath(referenceHistory.After.AbsolutePath);
					}
					else
					{
						referenceHistory.After.RelativePath = referenceHistory.Before.RelativePath;
						referenceHistory.After.AbsolutePath = referenceHistory.Before.AbsolutePath;
					}

					historyInformations.Add(referenceHistory);
				}

				ProjectReferences.Add(solutionReference.Before.AbsolutePath, historyInformations);
			}
		}

		private async Task CollectSolutionInformationAsync()
		{
			var solutionReader = new SolutionFileProcessor();
			var solutionReferences = solutionReader.Process(await Context.Options.FileSystem.ReadAsync(Context.SolutionPath));
			foreach (var solutionReference in solutionReferences)
			{
				var solutionHistory = new HistoryInformation();
				var solutionPathMapper = new PathMapper(Context.SolutionPath);
				solutionHistory.Before.AbsolutePath = solutionPathMapper.GetAbsolutePath(solutionReference.RelativePath);
				solutionHistory.Before.RelativePath = solutionReference.RelativePath;

				if (Context.Projects.Contains(solutionHistory.Before.AbsolutePath))
				{
					// reference will be moved
					solutionHistory.After.AbsolutePath = solutionPathMapper.GetSuggestedPath(solutionHistory.Before.AbsolutePath, Context.DestinationPath);
					solutionHistory.After.RelativePath = solutionPathMapper.GetRelativePath(solutionHistory.After.AbsolutePath);
				}
				else
				{
					solutionHistory.After.AbsolutePath = solutionPathMapper.GetAbsolutePath(solutionReference.RelativePath);
					solutionHistory.After.RelativePath = solutionReference.RelativePath;
				}

				SolutionReferences.Add(solutionHistory);
			}
		}
	}
}