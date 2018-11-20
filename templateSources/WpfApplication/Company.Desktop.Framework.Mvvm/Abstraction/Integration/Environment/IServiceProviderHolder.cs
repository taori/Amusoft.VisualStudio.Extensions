using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment
{
	public interface IServiceProviderHolder
	{
		IServiceProvider ServiceProvider { get; set; }
	}
}