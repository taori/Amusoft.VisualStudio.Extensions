using Microsoft.EntityFrameworkCore.Design;

namespace Model.EntityFramework.Utility
{
	public class DesignTimeDefaultDbContextFactory : IDesignTimeDbContextFactory<DefaultDataContext>
	{
		/// <inheritdoc />
		public DefaultDataContext CreateDbContext(string[] args)
		{
			return DefaultDataContextFactory.Create(DefaultDataContextFactory.ContextType.Design);
		}
	}
}