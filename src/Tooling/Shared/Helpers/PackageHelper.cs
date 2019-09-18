using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Tooling.Utility
{
	public static class PackageHelper
	{
		public static DTE GetDTE()
		{
			return Package.GetGlobalService(typeof(DTE)) as DTE;
		}
	}
}