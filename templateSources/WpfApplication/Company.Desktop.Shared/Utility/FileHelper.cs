using System;
using System.IO;
using System.Linq;

namespace Company.Desktop.Shared.Utility
{
	public static class FileHelper
	{
		public static string DomainFileRoot =>
			Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"Company.Desktop"
			);

		public static string GetDomainFile(params string[] paths) => Path.Combine(new[] {DomainFileRoot}.Concat(paths).ToArray());
	}
}
