using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class WindowDeactivatorSession
	{
		public static readonly DependencyProperty CloseChecksPassedProperty = DependencyProperty.RegisterAttached(
			"CloseChecksPassed", typeof(bool), typeof(WindowDeactivatorSession), new PropertyMetadata(default(bool)));

		public static void SetCloseChecksPassed(DependencyObject element, bool value)
		{
			element.SetValue(CloseChecksPassedProperty, value);
		}

		public static bool GetCloseChecksPassed(DependencyObject element)
		{
			return (bool) element.GetValue(CloseChecksPassedProperty);
		}

		public CancelEventArgs CancelArgs { get; set; }

		/// <inheritdoc />
		public WindowDeactivatorSession(CancelEventArgs cancelArgs)
		{
			CancelArgs = cancelArgs;
			CancelArgs.Cancel = true;
		}

		public async Task<bool> IsCancelledAsync(IDeactivate deactivate, IServiceProvider serviceProvider)
		{
			if (deactivate == null)
				return false;

			var deactivationContext = new DeactivationContext(serviceProvider);
			await deactivate.DeactivateAsync(deactivationContext);
			if (deactivationContext.Cancelled)
			{
				CancelArgs.Cancel = true;
				return true;
			}

			return false;
		}

		public async Task<bool> IsCancelledAsync(IInteractive interactive, IServiceProvider serviceProvider)
		{
			if (interactive == null)
				return false;

			var closeContext = new WindowCloseContext(interactive, serviceProvider);
			await interactive.ExecuteBehavioursAsync<IWindowCloseBehaviour, IWindowCloseBehaviourContext>(closeContext);

			if (closeContext.Cancelled)
			{
				CancelArgs.Cancel = true;
				return true;
			}

			return false;
		}
	}
}