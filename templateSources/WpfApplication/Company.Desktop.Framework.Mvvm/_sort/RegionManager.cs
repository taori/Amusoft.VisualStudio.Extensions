using System;
using System.Collections.Generic;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using NLog;

namespace Company.Desktop.Framework.Mvvm._sort
{
	public class RegionManager : IRegionManager
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(RegionManager));

		public IViewModelActivator ViewModelActivator { get; }

		public RegionManager(IViewModelActivator viewModelActivator)
		{
			ViewModelActivator = viewModelActivator;
		}

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
				AddRegister(newViewModel, regionName, control);
			}
			else
			{
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
				regionControlDictionary.Add(regionName, control);
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
	}
}