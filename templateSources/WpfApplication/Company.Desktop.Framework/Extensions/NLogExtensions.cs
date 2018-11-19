using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Logging;
using NLog;

namespace Company.Desktop.Framework.Extensions
{
	public static class NLogExtensions
	{
		public static void System(this ILogger source, string message, LogLevel logLevel, params (string, object)[] values)
		{
			Custom(source, message, logLevel, MetaType.System, values);
		}

		public static void Custom(this ILogger source, string message, LogLevel logLevel, MetaType type, params (string, object)[] values)
		{
			Custom(source, message, logLevel, type, values.ToDictionary(d => d.Item1, d => d.Item2));
		}

		public static void Custom(this ILogger source, string message, LogLevel logLevel, MetaType type, Dictionary<string, object> values = null)
		{
			var logEventInfo = new LogEventInfo(logLevel, "", message);
			if (values != null)
			{
				foreach (var pair in values)
				{
					logEventInfo.Properties[pair.Key] = pair.Value;
				}
			}

			logEventInfo.Properties["METATYPE"] = type.ToString();
			source.Log(logEventInfo);
		}
	}
}