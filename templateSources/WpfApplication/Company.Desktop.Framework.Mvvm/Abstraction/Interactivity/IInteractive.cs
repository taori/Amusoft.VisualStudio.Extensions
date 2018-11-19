using System.Collections.Generic;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IInteractive
	{
		List<IBehaviour> Behaviours { get; }
	}
}