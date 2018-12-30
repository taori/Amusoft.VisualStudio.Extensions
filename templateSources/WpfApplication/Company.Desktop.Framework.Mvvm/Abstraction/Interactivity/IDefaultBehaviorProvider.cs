using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IDefaultBehaviorProvider
	{
		IEnumerable<IBehavior> GetDefaultBehaviors();
	}
}