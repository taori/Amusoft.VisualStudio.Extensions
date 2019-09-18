﻿using System;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.VisualStudio.Shell;

[assembly: NeutralResourcesLanguage("en")]
[assembly: ThemeInfo(
	ResourceDictionaryLocation.None,
	ResourceDictionaryLocation.SourceAssembly
)]
[assembly: XmlnsPrefix("http://schemas.tooling.ui.wpf.com/winfx/2006/xaml/presentation", "localUI")]
[assembly: XmlnsDefinition("http://schemas.tooling.ui.wpf.com/winfx/2006/xaml/presentation", "Tooling.Shared.Controls")]

namespace Tooling.Utility
{
	public static class Theme
	{
		private static ResourceDictionary BuildThemeResources()
		{
			ResourceDictionary allResources = new ResourceDictionary();
			ResourceDictionary shellResources = (ResourceDictionary)Application.LoadComponent(new Uri("Microsoft.VisualStudio.Platform.WindowManagement;component/Themes/ThemedDialogDefaultStyles.xaml", UriKind.Relative));
			ResourceDictionary scrollStyleContainer = (ResourceDictionary)Application.LoadComponent(new Uri("Microsoft.VisualStudio.Shell.UI.Internal;component/Styles/ScrollBarStyle.xaml", UriKind.Relative));
			ResourceDictionary localThemingContainer = (ResourceDictionary)Application.LoadComponent(new Uri("Tooling;component/Shared/Controls/Theme.xaml", UriKind.Relative));
			allResources.MergedDictionaries.Add(shellResources);
			allResources.MergedDictionaries.Add(scrollStyleContainer);
			allResources.MergedDictionaries.Add(localThemingContainer);
			allResources[typeof(ScrollViewer)] = new Style
			{
				TargetType = typeof(ScrollViewer),
				BasedOn = (Style)scrollStyleContainer[VsResourceKeys.ScrollViewerStyleKey]
			};
			return allResources;
		}

		private static ResourceDictionary ThemeResources { get; } = BuildThemeResources();

		public static void ShouldBeThemed(this FrameworkElement control)
		{
			if (control.Resources == null)
			{
				control.Resources = ThemeResources;
			}
			else if (control.Resources != ThemeResources)
			{
				ResourceDictionary d = new ResourceDictionary();
				d.MergedDictionaries.Add(ThemeResources);
				d.MergedDictionaries.Add(control.Resources);
				control.Resources = null;
				control.Resources = d;
			}
		}
	}
}