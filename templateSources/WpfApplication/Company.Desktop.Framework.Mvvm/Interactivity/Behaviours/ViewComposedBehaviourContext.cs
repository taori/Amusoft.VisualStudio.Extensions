﻿using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ViewComposedBehaviourContext : IViewComposedBehaviourContext
	{
		public IViewCompositionContext CompositionContext { get; }
		public IServiceContext ServiceContext { get; }

		public ViewComposedBehaviourContext([NotNull] IViewCompositionContext compositionContext, [NotNull] IServiceContext serviceContext)
		{
			CompositionContext = compositionContext ?? throw new ArgumentNullException(nameof(compositionContext));
			ServiceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
		}
	}
}