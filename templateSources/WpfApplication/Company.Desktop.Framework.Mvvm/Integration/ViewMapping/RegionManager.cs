using System;
using System.Collections.Generic;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Extensions;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class RegionManager : IRegionManager
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(RegionManager));

		public static readonly DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached(
			"RegionName", typeof(string), typeof(RegionManager), new FrameworkPropertyMetadata(default(string)){ PropertyChangedCallback = RegionNameChanged});

		private static void RegionNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UpdateRegister(d as FrameworkElement, GetRegionName(d), GetViewModel(d));
		}

		public static void SetRegionName(DependencyObject element, string value)
		{
			element.SetValue(RegionNameProperty, value);
		}

		public static string GetRegionName(DependencyObject element)
		{
			return (string) element.GetValue(RegionNameProperty);
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.RegisterAttached(
			"ViewModel", typeof(object), typeof(RegionManager), new FrameworkPropertyMetadata(default(object)){ PropertyChangedCallback = ViewModelChanged});

		private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UpdateRegister(d as FrameworkElement, GetRegionName(d), e.NewValue, e.OldValue);
		}

		public static void SetViewModel(DependencyObject element, object value)
		{
			element.SetValue(ViewModelProperty, value);
		}

		public static object GetViewModel(DependencyObject element)
		{
			return (object) element.GetValue(ViewModelProperty);
		}

		private static void UpdateRegister(FrameworkElement control, string regionName, object newViewModel, object oldViewModel = null)
		{
			if (oldViewModel != null)
			{
				Log.Debug($"Removing {oldViewModel.GetType().FullName}");
				Register.Remove(oldViewModel);
				Log.Debug($"Adding register entry for \"{regionName}\"");
				AddRegister(newViewModel, regionName, control);
			}
			else
			{
				Log.Debug($"Adding register entry for \"{regionName}\"");
				AddRegister(newViewModel, regionName, control);
			}
		}

		private static void AddRegister(object newViewModel, string regionName, FrameworkElement control)
		{
			if(newViewModel == null)
				return;
			
			if(!Register.TryGetValue(newViewModel, out var regionControlDictionary))
			{
				Log.Debug($"Creating region register for {newViewModel.GetType().FullName}");
				regionControlDictionary = new Dictionary<string, FrameworkElement>();
				Register.Add(newViewModel, regionControlDictionary);
			}

			if (regionName != null)
			{
				Log.Debug($"Registering control [{control.GetType().FullName}] as region [{regionName}] for viewmodel {newViewModel.GetType().FullName}");
				control.Unloaded += ControlOnUnloaded;
				regionControlDictionary.Add(regionName, control);
			}
		}

		private static void ControlOnUnloaded(object sender, RoutedEventArgs e)
		{
			// countermeasure for memory leaks
			if (sender != null && sender is FrameworkElement frameworkElement)
			{
				frameworkElement.Unloaded -= ControlOnUnloaded;
				var viewModel = GetViewModel(frameworkElement);
				var regionName = GetRegionName(frameworkElement);

				if (viewModel != null && Register.TryGetValue(viewModel, out var register))
				{
					register.Remove(regionName);
					if (register.Count == 0)
						Register.Remove(viewModel);
				}
			}
		}

		/// <summary>
		/// Register of data in the form of &lt;viewModel, &lt;regionName, control&gt;&gt;
		/// </summary>
		private static Dictionary<object, Dictionary<string, FrameworkElement>> Register { get; } = new Dictionary<object, Dictionary<string, FrameworkElement>>();
		
		/// <inheritdoc />
		public FrameworkElement GetControl(object regionViewModelHolder, string regionName)
		{
			if (regionViewModelHolder == null)
				throw new ArgumentNullException(nameof(regionViewModelHolder), $"{nameof(regionViewModelHolder)}");
			if (regionName == null)
				throw new ArgumentNullException(nameof(regionName), $"{nameof(regionName)}");

			if (!Register.TryGetValue(regionViewModelHolder, out var localRegister))
			{
				Log.Error($"Unable to get register entry for {regionViewModelHolder.ToString()}");
				return null;
			}

			if (!localRegister.TryGetValue(regionName, out var currentView))
			{
				Log.Error($"Unable to get register entry for {regionViewModelHolder.ToString()}");
				return null;
			}

			return currentView;
		}

		/// <inheritdoc />
		public FrameworkElement GetHostingWindow(object viewModel)
		{
			foreach (var pair in Register)
			{
				foreach (var frameworkElementPair in pair.Value)
				{
					if (ReferenceEquals(frameworkElementPair.Value.DataContext, viewModel))
					{
						var window = frameworkElementPair.Value.GetParentOfType<Window>();
						if (window != null)
							return window;
					}
				}
			}

			return null;
		}
	}
}