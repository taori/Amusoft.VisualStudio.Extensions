using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using MahApps.Metro.Controls;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class MetroTransititionControlBinder : IViewContextBinder
	{
		/// <inheritdoc />
		public bool TryBind(IViewCompositionContext context)
		{
			if (context.Control is TransitioningContentControl control)
			{
				if (context.DataContext is IViewTransition transition)
				{
					if (Enum.TryParse(transition.GetTransition().ToString(), out TransitionType parsed))
					{
						control.Transition = parsed;
					}
					else
					{
						control.Transition = TransitionType.Up;
					}
				}
				else
				{
					control.Transition = TransitionType.Up;
				}

				control.Content = context.DataContext;
				return true;
			}

			return false;
		}
	}
}