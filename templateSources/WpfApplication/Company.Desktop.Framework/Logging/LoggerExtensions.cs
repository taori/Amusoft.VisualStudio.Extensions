using NLog;

namespace Company.Desktop.Framework.Logging
{
	public static class LoggerExtensions
	{
		public static IInteractiveLogger Wrap(this ILogger source)
		{
			return new NLogInteractiveWrapper(source);
		}
	}
}