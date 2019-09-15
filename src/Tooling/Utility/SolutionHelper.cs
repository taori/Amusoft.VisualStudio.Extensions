using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace Tooling.Utility
{
	public static class SolutionHelper
	{
		public static DTE2 GetActiveIDE()
		{
			// Get an instance of currently running Visual Studio IDE.
			DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
			return dte2;
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
	}
}