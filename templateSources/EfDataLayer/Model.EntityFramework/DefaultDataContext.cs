using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.EntityFramework
{
	public class DefaultDataContext : DbContext
	{
		/// <inheritdoc />
		public DefaultDataContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<World> Worlds { get; set; }

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
#if DEBUG
			optionsBuilder.EnableDetailedErrors();
			optionsBuilder.EnableSensitiveDataLogging();
#endif
		}
	}
}