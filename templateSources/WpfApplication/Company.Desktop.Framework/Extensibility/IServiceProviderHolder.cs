using System;

namespace Company.Desktop.Framework.Extensibility
{
	public interface IServiceProviderHolder
	{
		IServiceProvider ServiceProvider { get; set; }
	}
}