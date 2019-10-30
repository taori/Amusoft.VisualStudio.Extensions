using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Tooling.Features.ProjectMover.Mapping;
using Tooling.Features.ProjectMover.Utility;
using Tooling.Shared.Parsers;
using Tooling.Utility;
using Task = System.Threading.Tasks.Task;

namespace Tooling.Features.ProjectMover
{
	public class MoverTool
	{
		public MoverTool(IEnumerable<string> projects, string solutionPath, string destinationPath, MoverToolOptions options = null)
		{
			Context = new MoverToolContext(projects, solutionPath, destinationPath, options ?? new MoverToolOptions());
		}

		public MoverToolContext Context { get; set; }

		public List<HistoryInformation> SolutionReferences { get; set; } = new List<HistoryInformation>();

		public Dictionary<string, List<HistoryInformation>> ProjectReferences { get; set; } = new Dictionary<string, List<HistoryInformation>>();
		
		public async Task MoveAsync()
		{
			await CollectInformationAsync();
			await RewriteProjectsAsync();
			await RewriteSolutionAsync();
			MoveFolders();
		}
		
		private void MoveFolders()
		{
			foreach (var projectReference in SolutionReferences)
			{
				if (projectReference.Before.AbsolutePath != projectReference.After.AbsolutePath)
				{
					var source = Path.GetDirectoryName(projectReference.Before.AbsolutePath);
					var target = Path.GetDirectoryName(projectReference.After.AbsolutePath);
					Context.Options.FileSystem.MoveDirectory(source, target);

					if (!string.Equals(projectReference.Before.AbsolutePath, projectReference.After.AbsolutePath, StringComparison.OrdinalIgnoreCase))
					{
						var directoryName = Path.GetDirectoryName(projectReference.After.AbsolutePath);
						var currentFileName = Path.GetFileName(projectReference.Before.AbsolutePath);
						var futureFileName = Path.GetFileName(projectReference.After.AbsolutePath);

						if (Context.Options.FileSystem.Exists(Path.Combine(directoryName, currentFileName)))
							Context.Options.FileSystem.MoveFile(Path.Combine(directoryName, currentFileName), Path.Combine(directoryName, futureFileName));
					}
				}
			}
		}

		private static readonly Regex SolutionReplaceRegex = new Regex("");
		private async Task RewriteSolutionAsync()
		{
			var solutionContent = new StringBuilder(await Context.Options.FileSystem.ReadAsync(Context.SolutionPath).ConfigureAwait(false));

			foreach (var projectReference in SolutionReferences)
			{
				if (projectReference.Before.AbsolutePath != projectReference.After.AbsolutePath)
				{
					solutionContent.Replace($@"""{projectReference.Before.RelativePath}""", $@"""{projectReference.After.RelativePath}""");
					
					HandleReferenceRenames(projectReference, solutionContent);
				}
			}

			await Context.Options.FileSystem.WriteAsync(Context.SolutionPath, solutionContent.ToString(), Encoding.UTF8).ConfigureAwait(false);
		}

		private static void HandleReferenceRenames(HistoryInformation projectReference, StringBuilder solutionContent)
		{
			if (!string.Equals(Path.GetFileName(projectReference.Before.RelativePath), Path.GetFileName(projectReference.After.RelativePath), StringComparison.OrdinalIgnoreCase))
			{
				solutionContent.Replace(
					$@"""{Path.GetFileNameWithoutExtension(projectReference.Before.RelativePath)}""",
					$@"""{Path.GetFileNameWithoutExtension(projectReference.After.RelativePath)}"""
				);
			}
		}

		private async Task RewriteProjectsAsync()
		{
			foreach (var projectReference in ProjectReferences)
			{
				if(projectReference.Value.Count == 0)
					continue;

				var content = await Context.Options.FileSystem.ReadAsync(projectReference.Key);
				var sb = new StringBuilder(content);
				foreach (var information in projectReference.Value)
				{
					sb.Replace(information.Before.RelativePath, information.After.RelativePath);
				}

				await Context.Options.FileSystem.WriteAsync(projectReference.Key, sb.ToString(), Encoding.UTF8);
			}
		}

		private async Task CollectInformationAsync()
		{
			await CollectSolutionInformationAsync();
			await CollectProjectsInformationAsync();
		}

		private async Task CollectProjectsInformationAsync()
		{
			foreach (var currentProject in SolutionReferences)
			{
				var projectProcessor = new ProjectReferenceParser();
				var projectReferences = projectProcessor.Process(await Context.Options.FileSystem.ReadAsync(currentProject.Before.AbsolutePath));
				var historyInformations = new List<HistoryInformation>();

				foreach (var projectReference in projectReferences)
				{
					var referenceHistory = new HistoryInformation();
					referenceHistory.Before.RelativePath = projectReference.RelativePath;
					referenceHistory.Before.AbsolutePath = new PathMapper(currentProject.Before.AbsolutePath).GetAbsolutePath(projectReference.RelativePath);

					var futureReference = SolutionReferences.FirstOrDefault(d => string.Equals(d.Before.AbsolutePath, referenceHistory.Before.AbsolutePath, StringComparison.OrdinalIgnoreCase));
					if (futureReference == null)
					{
						referenceHistory.After.AbsolutePath = referenceHistory.Before.AbsolutePath;
						referenceHistory.After.RelativePath = referenceHistory.Before.RelativePath;
					}
					else
					{
						referenceHistory.After.AbsolutePath = futureReference.After.AbsolutePath;
						referenceHistory.After.RelativePath = new PathMapper(currentProject.After.AbsolutePath).GetRelativePath(futureReference.After.AbsolutePath);
					}

					historyInformations.Add(referenceHistory);
				}

				ProjectReferences.Add(currentProject.Before.AbsolutePath, historyInformations);
			}
		}

		private async Task CollectSolutionInformationAsync()
		{
			var solutionReader = new SolutionReferenceParser();
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
					solutionHistory.After.AbsolutePath = Context.Options.ProjectPathTransformer.AbsolutePath(solutionPathMapper.GetSuggestedPath(solutionHistory.Before.AbsolutePath, Context.DestinationPath));
					solutionHistory.After.RelativePath = Context.Options.ProjectPathTransformer.RelativePath(solutionPathMapper.GetRelativePath(solutionHistory.After.AbsolutePath));
				}
				else
				{
					solutionHistory.After.AbsolutePath = Context.Options.ProjectPathTransformer.AbsolutePath(solutionPathMapper.GetAbsolutePath(solutionReference.RelativePath));
					solutionHistory.After.RelativePath = Context.Options.ProjectPathTransformer.RelativePath(solutionReference.RelativePath);
				}

				SolutionReferences.Add(solutionHistory);
			}
		}
	}
}