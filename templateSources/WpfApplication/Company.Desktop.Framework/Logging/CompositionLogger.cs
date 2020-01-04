using System;
using System.Collections.Generic;

namespace Company.Desktop.Framework.Logging
{
	public class CompositionLogger : IInteractiveLogger
	{
		private readonly HashSet<IInteractiveLogger> _loggers;

		public CompositionLogger(IEnumerable<IInteractiveLogger> loggers)
		{
			_loggers = new HashSet<IInteractiveLogger>(loggers);
		}

		/// <inheritdoc />
		public CompositionLogger() : this(Array.Empty<IInteractiveLogger>())
		{
		}

		public void AddLogger(IInteractiveLogger logger) => _loggers.Add(logger);

		/// <inheritdoc />
		public void Error(Exception exception)
		{
			foreach (var logger in _loggers)
			{
				logger.Error(exception);
			}
		}

		/// <inheritdoc />
		public void Error(string message)
		{
			foreach (var logger in _loggers)
			{
				logger.Error(message);
			}
		}

		/// <inheritdoc />
		public void Debug(string message)
		{
			foreach (var logger in _loggers)
			{
				logger.Debug(message);
			}
		}

		/// <inheritdoc />
		public void Warning(string message)
		{
			foreach (var logger in _loggers)
			{
				logger.Warning(message);
			}
		}

		/// <inheritdoc />
		public void Information(string message)
		{
			foreach (var logger in _loggers)
			{
				logger.Information(message);
			}
		}

		/// <inheritdoc />
		public void Fatal(Exception exception)
		{
			foreach (var logger in _loggers)
			{
				logger.Fatal(exception);
			}
		}

		/// <inheritdoc />
		public void Fatal(string message)
		{
			foreach (var logger in _loggers)
			{
				logger.Fatal(message);
			}
		}

		/// <inheritdoc />
		public void Trace(string message)
		{
			foreach (var logger in _loggers)
			{
				logger.Trace(message);
			}
		}
	}
}