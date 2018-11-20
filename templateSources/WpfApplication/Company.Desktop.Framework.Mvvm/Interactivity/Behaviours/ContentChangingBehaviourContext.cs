using System;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ContentChangingBehaviourContext : IContentChangingBehaviourContext
	{
		/// <inheritdoc />
		public ContentChangingBehaviourContext(IServiceProvider serviceProvider, object oldViewModel, object newViewModel)
		{
			OldViewModel = oldViewModel;
			NewViewModel = newViewModel;
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public object OldViewModel { get; }

		/// <inheritdoc />
		public object NewViewModel { get; }

		/// <inheritdoc />
		public bool Cancelled { get; private set; }

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public void Cancel()
		{
			Cancelled = true;
		}
	}
}