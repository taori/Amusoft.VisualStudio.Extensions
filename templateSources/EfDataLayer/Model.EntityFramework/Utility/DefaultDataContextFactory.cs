using Microsoft.EntityFrameworkCore;

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
			var database = $"Model.{type}";
			var options = new DbContextOptionsBuilder<DefaultDataContext>();
			options
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={database};Trusted_Connection=True;", b => b.CommandTimeout(60));
			return new DefaultDataContext(options.Options);
		}
	}
}