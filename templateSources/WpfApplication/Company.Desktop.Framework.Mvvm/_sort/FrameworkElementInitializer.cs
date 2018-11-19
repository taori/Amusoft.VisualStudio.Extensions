﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.Framework.Mvvm._sort
{
	public class FrameworkElementInitializer<TViewModel> : ViewModelInitializerBase<TViewModel, FrameworkElement>
		where TViewModel : ContentViewModel
	{
		/// <inheritdoc />
		public FrameworkElementInitializer(ViewModelInitializerContext context, FrameworkElement control, TViewModel viewModel) : base(context, control, viewModel)
		{
		}

		/// <inheritdoc />
		protected override void AttachUnloadBehaviour()
		{
		}

		/// <inheritdoc />
		protected override void AttachBehaviours(List<IBehaviour> behaviours)
		{
		}

		/// <inheritdoc />
		protected override void InitializeControl()
		{
//			Control.Background = new SolidColorBrush(Colors.Orange);
		}

		/// <inheritdoc />
		protected override async Task<bool> OnActivateAsync()
		{
			if (Control is ContentPresenter presenter)
			{
				presenter.Content = ViewModel;
			}

			return true;
		}
	}
}