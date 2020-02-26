using System;
using System.Collections.Generic;
using System.Windows;
using Company.Desktop.Framework.Mvvm.UI;
using MahApps.Metro.Controls;
using NLog;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class TabManager : ITabControllerManager
	{
		private static IServiceProvider _serviceProvider;

		public TabManager(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(TabManager));

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.RegisterAttached(
			"ViewModel", typeof(object), typeof(TabManager), new PropertyMetadata(default(object), ControllerChanged));

		public static void SetViewModel(DependencyObject element, object value)
		{
			if (value != null || GetViewModel(element) == null)
				element.SetValue(ViewModelProperty, value);
		}

		public static object GetViewModel(DependencyObject element)
		{
			return (object)element.GetValue(ViewModelProperty);
		}

		public static readonly DependencyProperty ControllerNameProperty = DependencyProperty.RegisterAttached(
			"ControllerName", typeof(string), typeof(TabManager), new PropertyMetadata(default(string), ControllerNameChanged));

		public static void SetControllerName(DependencyObject element, string value)
		{
			element.SetValue(ControllerNameProperty, value);
		}

		public static string GetControllerName(DependencyObject element)
		{
			return (string)element.GetValue(ControllerNameProperty);
		}

		private static ITabController CreateTabController(object tabControl)
		{
			if (tabControl is MetroTabControl metroTabControl)
			{
				Log.Debug("Creating Metro Tab Controller adapter.");
				return new MetroTabControllerAdapter(metroTabControl, _serviceProvider);
			}

			throw new NotSupportedException();
		}

		private static readonly Dictionary<(string name, object viewModel), ITabController> ControllerRegister = new Dictionary<(string name, object viewModel), ITabController>();

		private static void RegisterTabController(DependencyObject control, object viewModel, string controllerName)
		{
			var tabController = CreateTabController(control);
			Log.Debug($"Registering tab controller with the name {controllerName}.");
			if (tabController != null)
				ControllerRegister.Add((controllerName, viewModel), tabController);
		}

		private static void ControllerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RegisterTabController(d, GetViewModel(d), GetControllerName(d));
		}

		private static void ControllerNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RegisterTabController(d, GetViewModel(d), GetControllerName(d));
		}

		public bool TryGetController(object viewModel, string name, out ITabController controller)
		{
			if (ControllerRegister.TryGetValue((name, viewModel), out controller) && controller.IsOperational)
			{
				return true;
			}

			return false;
		}
	}
}