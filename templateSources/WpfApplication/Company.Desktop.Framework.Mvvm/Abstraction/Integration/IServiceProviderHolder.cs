using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration
{
	public interface IServiceProviderHolder
	{
		IServiceProvider ServiceProvider { get; set; }
	}
}