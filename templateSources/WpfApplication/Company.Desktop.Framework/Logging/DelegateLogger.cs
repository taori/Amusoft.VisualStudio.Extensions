using System;

namespace Company.Desktop.Framework.Logging
{
	public class DelegateLogger : IInteractiveLogger
	{
		/// <inheritdoc />
		public DelegateLogger()
		{
		}

		/// <inheritdoc />
		public DelegateLogger(Action<string> information, Action<string> warning, Action<string> error, Action<string> fatal)
		{
			Information = information;
			Warning = warning;
			Error = error;
			Fatal = fatal;
		}

		public Action<string> Information { get; set; }
		public Action<string> Warning { get; set; }
		public Action<string> Error { get; set; }
		public Action<string> Fatal { get; set; }
		public Action<string> Debug { get; set; }
		public Action<string> Trace { get; set; }

		/// <inheritdoc />
		void IInteractiveLogger.Error(Exception exception)
		{
			Error?.Invoke(exception.ToString());
		}

		/// <inheritdoc />
		void IInteractiveLogger.Error(string message)
		{
			Error?.Invoke(message);
		}

		/// <inheritdoc />
		void IInteractiveLogger.Debug(string message)
		{
			Debug?.Invoke(message);
		}

		/// <inheritdoc />
		void IInteractiveLogger.Warning(string message)
		{
			Warning?.Invoke(message);
		}

		/// <inheritdoc />
		void IInteractiveLogger.Information(string message)
		{
			Information?.Invoke(message);
		}

		/// <inheritdoc />
		void IInteractiveLogger.Fatal(Exception exception)
		{
			Fatal?.Invoke(exception.ToString());
		}

		/// <inheritdoc />
		void IInteractiveLogger.Fatal(string message)
		{
			Fatal?.Invoke(message);
		}

		/// <inheritdoc />
		void IInteractiveLogger.Trace(string message)
		{
			Trace?.Invoke(message);
		}
	}
}