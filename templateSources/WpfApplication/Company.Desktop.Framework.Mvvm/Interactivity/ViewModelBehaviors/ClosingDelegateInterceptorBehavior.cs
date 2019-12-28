using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	/// <summary>
	/// Behavior used to delegate the confirmation of whether the window can be closed or not. true for close, false for cancel
	/// </summary>
	public class ClosingDelegateInterceptorBehavior : InterceptClosingBehaviorBase
	{
		/// <summary>
		/// Callback used to delegate the confirmation of whether the window can be closed or not. true for close, false for cancel
		/// </summary>
		public Func<IWindowClosingBehaviorContext, Task<bool>> Callback { get; }

		/// <summary>
		/// Constructor used to delegate the confirmation of whether the window can be closed or not. true for close, false for cancel
		/// </summary>
		/// <param name="callback">delegate used to intercept, true for close, false for cancel</param>
		public ClosingDelegateInterceptorBehavior([NotNull] Func<IWindowClosingBehaviorContext, Task<bool>> callback)
		{
			Callback = callback ?? throw new ArgumentNullException(nameof(callback));
		}

		/// <inheritdoc />
		protected override async Task<bool> ShouldCancelAsync(IWindowClosingBehaviorContext argument)
		{
			return !await Callback(argument);
		}
	}
}