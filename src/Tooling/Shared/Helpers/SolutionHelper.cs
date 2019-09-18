using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using EnvDTE;
using EnvDTE80;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Tooling.Utility
{
	public static class SolutionHelper
	{
		public static DTE2 GetActiveIDE()
		{
			DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
			return dte2;
		}

		public static async Task UnloadProjectAsync(Guid projectGuid)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
			if (Package.GetGlobalService(typeof(SVsSolution)) is IVsSolution4 solution)
			{
				var result = solution.UnloadProject(projectGuid, (int) _VSProjectUnloadStatus.UNLOADSTATUS_UnloadedByUser);
				ErrorHandler.ThrowOnFailure(result);
			}
		}

		public static async Task LoadProjectAsync(Guid projectGuid, __VSBSLFLAGS flags)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
			if (Package.GetGlobalService(typeof(SVsSolution)) is IVsSolution4 solution)
			{
				var result = solution.EnsureProjectIsLoaded(projectGuid, (uint) flags);
				ErrorHandler.ThrowOnFailure(result);
			}
		}

		public static Project GetCurrentProject()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
			if (dte2.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0)
			{
				return activeSolutionProjects.GetValue(0) as Project;
			}

			return null;
		}

		public static async Task<bool> IsIdeAndSolutionFileInSyncAsync()
		{
			var ide = GetActiveIDE();
			if (string.IsNullOrEmpty(ide.Solution.FileName))
				return true;

			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var solutionFile = SolutionFile.Parse(ide.Solution.FileName);
			var allProjects = SolutionHelper.GetProjectsRecursive();
			var solutionFileProjects = solutionFile.ProjectsInOrder.Where(d => d.ProjectType != SolutionProjectType.SolutionFolder).Select(d => d.AbsolutePath.ToUpperInvariant()).ToHashSet();
			var vsRuntimeProjects = allProjects.Select(d => d.FullName.ToUpperInvariant()).ToHashSet();

			return vsRuntimeProjects.All(d => solutionFileProjects.Contains(d))
			       && solutionFileProjects.All(d => vsRuntimeProjects.Contains(d));
		}

		public static IEnumerable<Project> GetProjectsRecursive()
		{
			Projects projects = GetActiveIDE().Solution.Projects;
			var item = projects.GetEnumerator();
			while (item.MoveNext())
			{
				var project = item.Current as Project;
				if (project == null)
				{
					continue;
				}

				if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
				{
					foreach (var sub in GetSolutionFolderProjects(project))
					{
						yield return sub;
					}
				}
				else
				{
					yield return project;
				}
			}
		}

		private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
		{
			for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
			{
				var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
				if (subProject == null)
				{
					continue;
				}

				// If this is another solution folder, do a recursive call, otherwise add
				if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
				{
					foreach (var sub in GetSolutionFolderProjects(subProject))
					{
						yield return sub;
					}
				}
				else
				{
					yield return subProject;
				}
			}
		}

		public static void SaveSolution()
		{
			var solution = PackageHelper.GetDTE().Solution;
			if (!string.IsNullOrEmpty(solution.FullName))
			{
				solution.SaveAs(solution.FullName);
			}
		}
	}
}