﻿using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class InterceptClosingBehaviourBase : BehaviourBase, IWindowClosingBehaviour
	{
		protected abstract Task<bool> ShouldCancelAsync();
		
		/// <inheritdoc />
		public IWindowClosingBehaviourContext Context { get; private set; }

		/// <inheritdoc />
		public async Task ExecuteAsync()
		{
			if (await ShouldCancelAsync())
				Context.Cancel();

			RaiseExecuted();
		}

		/// <inheritdoc />
		public void SetContext(IWindowClosingBehaviourContext context)
		{
			Context = context;
		}
	}
}