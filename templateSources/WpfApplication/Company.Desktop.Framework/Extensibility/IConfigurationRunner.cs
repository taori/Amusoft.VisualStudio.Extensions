using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Extensibility
{
	[InheritedExport(typeof(IConfigurationRunner), LifeTime = LifeTime.Singleton)]
	public interface IConfigurationRunner
	{
		void Execute();
	}
}