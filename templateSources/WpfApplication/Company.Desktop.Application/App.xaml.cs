﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Company.Desktop.Application.Dependencies;
using Company.Desktop.Application.Dependencies.Configuration;
using Company.Desktop.Application.Dependencies.Logging;
using Company.Desktop.Application.Dependencies.Setup;
using Company.Desktop.Framework.Extensions;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Internal;

namespace Company.Desktop.Application
{
	public partial class App : System.Windows.Application
	{
		static App()
		{
			LogConfiguration.RegisterTargets();
			Log = LogManager.GetLogger(nameof(App));
			DependencyContainer = new DependencyContainer();
		}
		
		private static ILogger Log { get; }

		public static readonly DependencyContainer DependencyContainer;

		private static Mutex ApplicationMutex;

		/// <inheritdoc />
		protected override void OnStartup(StartupEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnStartup)}.", LogLevel.Trace);
			try
			{
				AttachAllExceptionHandlers();

				DependencyContainer.Configure();
				ShutdownMode = ShutdownMode.OnMainWindowClose;

				ApplicationMutex = new Mutex(false, typeof(App).FullName, out var mutexCreated);
				if (!mutexCreated)
				{
					Log.Warn($"Application already running. Shutting down.");
					MessageBox.Show($"Application already running. Shutting down.", "Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					Current.Shutdown(0);
				}

				var runners = DependencyContainer.ServiceProvider.GetServices<IConfigurationRunner>();
				foreach (var runner in runners)
				{
					Log.Debug($"Executing {nameof(IConfigurationRunner)} \"{runner.GetType().FullName}\".");
					runner.Execute();
				}
				var navigationService = DependencyContainer.ServiceProvider.GetService<INavigationService>();
				navigationService.OpenWindowAsync(new DefaultWindowViewModel(new MainViewModel()), nameof(MainViewModel));

				base.OnStartup(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnExit(ExitEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnExit)}.", LogLevel.Trace);
			try
			{
				Log.Debug($"Disposing {nameof(ApplicationMutex)}.");
				ApplicationMutex?.Dispose();
				base.OnExit(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnActivated(EventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnActivated)}.", LogLevel.Trace);
			try
			{
				base.OnActivated(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnDeactivated(EventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnDeactivated)}.", LogLevel.Trace);
			try
			{
				base.OnDeactivated(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnSessionEnding)}.", LogLevel.Trace);
			try
			{
				base.OnSessionEnding(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnLoadCompleted(NavigationEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnLoadCompleted)}.", LogLevel.Trace);
			try
			{
				base.OnLoadCompleted(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		private void AttachAllExceptionHandlers()
		{
			Log.Debug("Registering exception handler for AppDomain.CurrentDomain.UnhandledException.");
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
				LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

			Log.Debug("Registering exception handler for DispatcherUnhandledException.");
			DispatcherUnhandledException += (s, e) =>
				LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

			Log.Debug("Registering exception handler for TaskScheduler.UnobservedTaskException.");
			TaskScheduler.UnobservedTaskException += (s, e) =>
				LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
		}

		private void LogUnhandledException(Exception exception, string source)
		{
			Log.Error($"Unhandled exception ({source}): {exception.Expand()}");
		}
	}
}
