using System.Text.RegularExpressions;

namespace Company.Desktop.Framework.Extensions
{
	public static class RegexExtensions
	{
		public static bool TryMatch(this Regex source, string input, out Match match)
		{
			if (string.IsNullOrEmpty(input))
			{
				match = null;
				return false;
			}

			if (source.IsMatch(input))
			{
				match = source.Match(input);
				return true;
			}
			else
			{
				match = null;
				return false;
			}
		}
	}
}