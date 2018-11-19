using System;
using System.Collections.Generic;

namespace Company.Desktop.Framework.Extensions
{
	public static class ReflectionExtensions
	{
		public static IEnumerable<Type> GetTypeHierarchy(this Type source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source), $"{nameof(source)}");

			Type current = source;
			do
			{
				yield return current;
				current = current.BaseType;

			} while (current != null);
		}
	}
}