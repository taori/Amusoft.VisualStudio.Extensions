using System;
using System.Text;

namespace Company.Desktop.Framework.Extensions
{
	public static class ExceptionExtensions
	{
		public static string Expand(this Exception source)
		{
			var sb = new StringBuilder();
			Expand(sb, source);
			return sb.ToString();

			void Expand(StringBuilder builder, Exception exception)
			{
				if (!string.IsNullOrEmpty(exception.Message))
					builder.AppendLine(exception.Message);
				if (!string.IsNullOrEmpty(exception.StackTrace))
					builder.AppendLine(exception.StackTrace);
	
				if (exception is AggregateException agg)
				{
					foreach (var innerException in agg.InnerExceptions)
					{
						Expand(builder, innerException);
					}
				}
			}
		}
	}
}