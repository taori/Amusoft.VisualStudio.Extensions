using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Company.Desktop.Framework.Extensions
{
	public static class StringExtensions
	{
		public static bool TryMultiReplace(this string input, Dictionary<string, string> replacements, out string result)
		{
			var regex = new Regex(string.Join("|", replacements.Keys));
			if (!regex.IsMatch(input))
			{
				result = null;
				return false;
			}

			result = regex.Replace(input, m => replacements[m.Value]);
			return true;
		}
	}
}