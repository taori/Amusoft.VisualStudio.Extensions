using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Model.EntityFramework.Utility
{
	public static class DefaultDataContextFactory
	{
		public enum ContextType
		{
			Design,
			Real
		}

		public static DefaultDataContext Create(ContextType type)
		{
			var database = $"Model.EntityFramework.{type}";
			var options = new DbContextOptionsBuilder<DefaultDataContext>();
			options
				.UseLoggerFactory(new LoggerFactory(new ILoggerProvider[]{ CreateDebugLogger() }))
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.UseSqlServer(
					$"Server=(localdb)\\mssqllocaldb;Database={database};Trusted_Connection=True;MultipleActiveResultSets=true", 
					b => b.CommandTimeout(60));

			return new DefaultDataContext(options.Options);
		}

		private static ILoggerProvider CreateDebugLogger()
		{
#if DEBUG
			return new DebugLoggerProvider();
#else
			return NullLoggerProvider.Instance;
#endif
		}
	}
}