using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment
{
	public interface IServiceContext
	{
		IServiceProvider ServiceProvider { get; }
	}
}