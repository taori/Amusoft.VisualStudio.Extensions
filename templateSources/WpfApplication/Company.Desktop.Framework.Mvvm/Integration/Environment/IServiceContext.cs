using System;

namespace Company.Desktop.Framework.Mvvm.Integration.Environment
{
	public interface IServiceContext
	{
		IServiceProvider ServiceProvider { get; }
	}
}