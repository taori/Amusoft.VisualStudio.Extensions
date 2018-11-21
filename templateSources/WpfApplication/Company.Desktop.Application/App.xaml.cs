using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Company.Desktop.Application.Dependencies;
using Company.Desktop.Framework.Extensions;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Application
{
	public partial class App : System.Windows.Application
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(App));

		public static readonly DependencyContainer DependencyContainer = new DependencyContainer();

		private static Mutex ApplicationMutex;

		/// <inheritdoc />
		protected override void OnStartup(StartupEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnStartup)}.", LogLevel.Trace);
			try
			{
				AttachAllExceptionHandlers();

				DependencyContainer.Configure();
				this.ShutdownMode = ShutdownMode.OnMainWindowClose;

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
					runner.Execute();
				}

				var navigationService = DependencyContainer.ServiceProvider.GetService<INavigationService>();
				navigationService.OpenWindowAsync(new MainViewModel(), nameof(MainViewModel));
				
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
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
				LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

			DispatcherUnhandledException += (s, e) =>
				LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

			TaskScheduler.UnobservedTaskException += (s, e) =>
				LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
		}

		private void LogUnhandledException(Exception exception, string source)
		{
			string message = $"Unhandled exception ({source})";
			try
			{
				System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
				message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Exception in LogUnhandledException");
			}
			finally
			{
				Log.Error(exception, message);
			}
		}
	}
}
