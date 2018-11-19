using System.Collections.Generic;

namespace Company.Desktop.Framework.Mvvm
{
	public interface IInteractive
	{
		List<IBehaviour> Behaviours { get; }
	}
}