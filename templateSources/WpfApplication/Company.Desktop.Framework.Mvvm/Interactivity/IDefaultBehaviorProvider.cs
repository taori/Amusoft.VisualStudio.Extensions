using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;

namespace Company.Desktop.Framework.Mvvm.Interactivity
{
	public interface IDefaultBehaviorProvider
	{
		IEnumerable<IBehavior> GetDefaultBehaviors();
	}
}