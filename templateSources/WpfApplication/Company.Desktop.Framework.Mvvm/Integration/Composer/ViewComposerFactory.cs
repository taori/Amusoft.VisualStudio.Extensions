using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public class ViewComposerFactory : IViewComposerFactory
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewComposerFactory));

		public IEnumerable<IViewComposer> Composers { get; }

		public ViewComposerFactory(IEnumerable<IViewComposer> composers)
		{
			Composers = composers;
		}

		/// <inheritdoc />
		public IViewComposer Create(FrameworkElement control)
		{
			if (control == null) throw new ArgumentNullException(nameof(control));

			foreach (var composer in Composers.OrderByDescending(d => d.Priority))
			{
				if (composer.CanHandle(control))
					return composer;
			}

			Log.Error($"No implementation of {typeof(ViewComposerFactory).FullName} can process view type {control.GetType().FullName}.");
			return null;
		}
	}
}