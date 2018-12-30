﻿using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class DisposeOnCloseBehavior : AsyncBehaviorBase<IWindowClosedBehaviorContext>
	{
		/// <inheritdoc />
		protected override Task OnExecuteAsync(IWindowClosedBehaviorContext context)
		{
			if (context.ViewModel is IDisposable disposable)
				disposable.Dispose();

			return Task.CompletedTask;
		}
	}
}