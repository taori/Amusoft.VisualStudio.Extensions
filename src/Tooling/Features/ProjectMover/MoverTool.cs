using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.Shell;
using Tooling.Features.ProjectMover.Mapping;
using Tooling.Features.ProjectMover.Processors;
using Tooling.Features.ProjectMover.Utility;
using Tooling.Shared.Resources;
using Task = System.Threading.Tasks.Task;

namespace Tooling.Features.ProjectMover
{
	public static class MoverTool
	{
		public static async Task MoveAsync(IEnumerable<ProjectInSolution> projects, string solutionPath, string targetPath)
		{
			var context = BuildContext(projects, solutionPath, targetPath);
			await CollectInformationAsync(context);
			var relatedProjects = GetProjectRewriteCandidates(context).ToHashSet();
			await RewriteProjectsAsync(context, relatedProjects);
			await RewriteSolutionAsync(context);

			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
			await Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
			{
				if (MessageBox.Show(Translations.question_ClearTargetFolderOfEmptyFolders, Translations.caption_Question, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					RemoveEmptyFolders(targetPath);
				}
			}));
		}

		private static void RemoveEmptyFolders(string targetPath)
		{
			RemoveEmptyFoldersRecursive(targetPath);
		}

		private static void RemoveEmptyFoldersRecursive(string targetPath)
		{
			foreach (var directory in Directory.GetDirectories(targetPath))
			{
				RemoveEmptyFoldersRecursive(directory);
			}

			var dirInfo = new DirectoryInfo(targetPath);
			if (dirInfo.GetFiles().Length > 0)
				return;

			try
			{
				Directory.Delete(targetPath);
			}
			finally
			{
			}
		}

		private static async Task RewriteSolutionAsync(MoverToolContext context)
		{
			var content = new StringBuilder(File.ReadAllText(context.SolutionPath));
			var processor = new SolutionFileProcessor();
			List<SolutionReference> references;
			using (var streamReader = new StreamReader(context.SolutionPath))
			{
				references = await processor.ProcessAsync(streamReader);
			}

			var pathMapper = new PathMapper(context.SolutionPath);

			foreach (var reference in references)
			{
				var absolutePath = pathMapper.GetAbsolutePath(reference.RelativePath);
				if (context.PathSuggestions.TryGetValue(absolutePath, out var suggestion) 
				    && context.ProjectsForRewrite.Select(d => d.AbsolutePath).Any(d => string.Equals(absolutePath, d, StringComparison.OrdinalIgnoreCase)))
				{
					var newRelative = pathMapper.GetRelativePath(suggestion);
					content.Replace(reference.RelativePath, newRelative);
				}
			}

			File.WriteAllText(context.SolutionPath, content.ToString());
		}

		private static async Task RewriteProjectsAsync(MoverToolContext context, HashSet<string> relatedProjects)
		{
			foreach (var relatedPath in relatedProjects)
			{
				var projectProcessor = new ProjectFileProcessor();
				List<ProjectReference> containedReferences;
				using (var streamReader = new StreamReader(relatedPath))
				{
					containedReferences = await projectProcessor.ProcessAsync(streamReader);
				}

				if(containedReferences.Count == 0)
					continue;

				var content = new StringBuilder(File.ReadAllText(relatedPath));
				var projectMapper = new PathMapper(relatedPath);
				foreach (var containedReference in containedReferences)
				{
					var absoluteReference = projectMapper.GetAbsolutePath(containedReference.RelativePath);
					if (context.PathSuggestions.TryGetValue(absoluteReference, out var rewrite))
					{
						var newRelativePath = projectMapper.GetRelativePath(rewrite);
						content.Replace(containedReference.RelativePath, newRelativePath);
					}
				}
				
				File.WriteAllText(relatedPath, content.ToString());
			}

			foreach (var pathSuggestion in context.PathSuggestions)
			{
				var source = Path.GetDirectoryName(pathSuggestion.Key);
				var target = Path.GetDirectoryName(pathSuggestion.Value);
				Directory.Move(source, target);
			}
		}

		private static IEnumerable<string> GetProjectRewriteCandidates(MoverToolContext context)
		{
			return context
				.ProjectsForRewrite
				.SelectMany(d => context.AbsoluteSolutionReferences.GetInversedDependencies(d.AbsolutePath))
				.Concat(context.ProjectsForRewrite.Select(d => d.AbsolutePath))
				.ToHashSet();

		}

		private static async Task CollectInformationAsync(MoverToolContext context)
		{
			using (var streamReader = new StreamReader(context.SolutionPath, Encoding.UTF8))
			{
				context.RelativeSolutionReferences = await new SolutionFileProcessor().ProcessAsync(streamReader);
			}

			context.AbsoluteSolutionReferences = await GetAbsolutePathMapAsync(context);
			context.PathSuggestions = GetFuturePaths(context);
		}

		private static Dictionary<string, string> GetFuturePaths(MoverToolContext context)
		{
			var mapper = new PathMapper(context.SolutionPath);
			return context.ProjectsForRewrite.Select(d => new
			{
				currentPath = d.AbsolutePath,
				suggestedPath = mapper.GetSuggestedPath(d.AbsolutePath, context.TargetPath),
			}).ToDictionary(d => d.currentPath, d => d.suggestedPath);
		}

		private static async Task<AbsolutePathReferenceMap> GetAbsolutePathMapAsync(MoverToolContext context)
		{
			var map = new AbsolutePathReferenceMap();
			var pathMapper = new PathMapper(context.SolutionPath);
			var projectProcessor = new ProjectFileProcessor();

			foreach (var reference in context.RelativeSolutionReferences)
			{
				var absoluteReference = pathMapper.GetAbsolutePath(reference.RelativePath);

				List<ProjectReference> projectReferences;
				using (var streamReader = new StreamReader(absoluteReference, Encoding.UTF8))
				{
					projectReferences = await projectProcessor.ProcessAsync(streamReader);
				}

				var absoluteProjectReferences = new HashSet<string>();

				foreach (var projectReference in projectReferences)
				{
					var mapper = new PathMapper(absoluteReference);
					absoluteProjectReferences.Add(mapper.GetAbsolutePath(projectReference.RelativePath));
				}

				map.AddEntries(absoluteReference, absoluteProjectReferences);
			}

			return map;
		}

		private static MoverToolContext BuildContext(IEnumerable<ProjectInSolution> projects, string solutionPath, string targetPath)
		{
			var context = new MoverToolContext(projects, solutionPath, targetPath);
			return context;
		}
	}
}