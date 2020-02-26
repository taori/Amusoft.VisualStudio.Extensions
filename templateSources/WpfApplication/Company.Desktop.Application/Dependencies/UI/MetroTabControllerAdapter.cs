using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Commands;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.UI;
using MahApps.Metro.Controls;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class MetroTabControllerAdapter : ITabController
	{
		private readonly WeakReference<IServiceProvider> _serviceProviderReference;

		private readonly WeakReference<MetroTabControl> _metroTabControlReference;

		private MetroTabControl MetroTabControl =>
			_metroTabControlReference.TryGetTarget(out var control)
				? control
				: null;

		public MetroTabControllerAdapter(MetroTabControl metroTabControl, IServiceProvider serviceProvider)
		{
			_serviceProviderReference = new WeakReference<IServiceProvider>(serviceProvider);
			metroTabControl.Unloaded += MetroTabControlOnUnloaded;
			metroTabControl.TabItemClosingEvent += MetroTabControlOnTabItemClosingEvent;
			_metroTabControlReference = new WeakReference<MetroTabControl>(metroTabControl);
		}

		private void MetroTabControlOnTabItemClosingEvent(object sender, BaseMetroTabControl.TabItemClosingEventArgs e)
		{
			if (e.ClosingTabItem.Content is MetroContentControl contentControl)
			{
				if (contentControl.Content is ITab tab)
				{
					var result = Task.Run(() => tab.TryCloseTabAsync()).GetAwaiter().GetResult();
					e.Cancel = !result;
				}
			}
		}

		private void MetroTabControlOnUnloaded(object sender, RoutedEventArgs e)
		{
			if (sender is MetroTabControl control)
			{
				control.Unloaded -= MetroTabControlOnUnloaded;
				control.TabItemClosingEvent -= MetroTabControlOnTabItemClosingEvent;
			}
		}

		public bool IsOperational => _metroTabControlReference.TryGetTarget(out _);

		public bool IsFocused(ITab model)
		{
			if (MetroTabControl.SelectedContent is MetroContentControl contentControl)
			{
				if (contentControl.Content.Equals(model))
					return true;
			}

			return false;
		}

		public void Insert(int index, ITab model)
		{
			if (!IsOperational)
				return;

			var tab = new MetroTabItem();
			var contentControl = new MetroContentControl();
			contentControl.Content = model;
			tab.Loaded += TabOnLoaded;
			tab.Content = contentControl;
			tab.Header = model.Title ?? "New Tab";
			tab.CloseButtonEnabled = model.Closable;
			tab.CloseButtonMargin = new Thickness(0, 8, 0, 0);
			tab.CloseTabCommand = new TaskCommand(o => Task.CompletedTask);

			MetroTabControl.Items.Insert(index, tab);
		}

		private async void TabOnLoaded(object sender, RoutedEventArgs e)
		{
			if (sender is MetroTabItem tabItem)
			{
				tabItem.Loaded -= TabOnLoaded;

				if (tabItem.Content is MetroContentControl contentControl)
				{
					if (contentControl.Content is ITab tab)
					{
						if (!string.IsNullOrWhiteSpace(tab.Title))
							tabItem.Header = tab.Title;

						tab.WhenTitleChanged
							.ObserveOn(SynchronizationContext.Current)
							.Subscribe(newTitle =>
							{
								if (!string.IsNullOrWhiteSpace(newTitle))
									tabItem.Header = newTitle;
							});

						tab.WhenClosableChanged
							.ObserveOn(SynchronizationContext.Current)
							.Subscribe(canClose => { tabItem.CloseButtonEnabled = canClose; });
					}

					if (contentControl.Content is IActivateable activate)
					{
						if(!_serviceProviderReference.TryGetTarget(out var serviceProvider))
							throw new Exception("Service provider reference not found");

						await activate.ActivateAsync(new Framework.Mvvm.Interactivity.ActivationContext(serviceProvider));
					}
				}
			}
		}

		public int Add(ITab model)
		{
			if (!IsOperational)
				return -1;

			var count = MetroTabControl.Items.Count;
			Insert(count, model);
			return count;
		}

		public int FindIndex(ITab model)
		{
			bool ContainsModel(object theModel, object control)
			{
				if (control is MetroTabItem tabItem)
					return ContainsModel(theModel, tabItem.Content);
				if (control is MetroContentControl contentControl)
					return object.ReferenceEquals(theModel, contentControl.Content);

				return false;
			}

			for (int i = 0; i < MetroTabControl.Items.Count; i++)
			{
				var controlItem = MetroTabControl.Items[i];
				if (ContainsModel(model, controlItem))
					return i;
			}

			return -1;
		}

		public void Remove(ITab model)
		{
			if (!IsOperational)
				return;

			var index = FindIndex(model);
			if (index >= 0)
			{
				MetroTabControl.Items.RemoveAt(index);
			}
		}

		public void RemoveAt(int index)
		{
			if (!IsOperational)
				return;

			MetroTabControl.Items.RemoveAt(index);
		}

		public void Focus(ITab model)
		{
			if (!IsOperational)
				return;

			var index = FindIndex(model);
			if (index >= 0)
			{
				if (MetroTabControl.Items[index] is MetroTabItem tabItem)
				{
					tabItem.Focus();
				}
			}
		}


		public void FocusAt(int index)
		{
			if (!IsOperational)
				return;

			if (index < 0)
				return;

			if (MetroTabControl.Items[index] is MetroTabItem tabItem)
			{
				tabItem.Focus();
			}
		}

		public void FocusLast()
		{
			if (!IsOperational)
				return;

			if (MetroTabControl.Items.Count == 0)
				return;

			if (MetroTabControl.Items[MetroTabControl.Items.Count - 1] is MetroTabItem tabItem)
			{
				tabItem.Focus();
			}
		}

		public int TabCount => MetroTabControl?.Items?.Count ?? 0;
	}
}