using System;

namespace Company.Desktop.Framework.Mvvm.Integration.Environment
{
	public interface IServiceProviderHolder
	{
		IServiceProvider ServiceProvider { get; set; }
	}
}