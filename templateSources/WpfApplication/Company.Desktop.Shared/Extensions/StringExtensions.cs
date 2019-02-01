using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Desktop.Shared.Extensions
{
	public static class StringExtensions
	{
		public static string JoinOn(this IEnumerable<string> source, string separator) => string.Join(separator, source);
	}
}
