using System;
using JetBrains.Annotations;
using NLog;

namespace Company.Desktop.Framework.Logging
{
	internal class NLogInteractiveWrapper : IInteractiveLogger
	{
		[NotNull] private readonly ILogger _logger;

		public NLogInteractiveWrapper([NotNull] ILogger logger)
		{
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));

			_logger = logger;
		}

		/// <inheritdoc />
		public void Error(Exception exception)
		{
			_logger.Error(exception);
		}

		/// <inheritdoc />
		public void Error(string message)
		{
			_logger.Error(message);
		}

		/// <inheritdoc />
		public void Debug(string message)
		{
			_logger.Debug(message);
		}

		/// <inheritdoc />
		public void Warning(string message)
		{
			_logger.Warn(message);
		}

		/// <inheritdoc />
		public void Information(string message)
		{
			_logger.Info(message);
		}

		/// <inheritdoc />
		public void Fatal(Exception exception)
		{
			_logger.Fatal(exception);
		}

		/// <inheritdoc />
		public void Fatal(string message)
		{
			_logger.Fatal(message);
		}

		/// <inheritdoc />
		public void Trace(string message)
		{
			_logger.Trace(message);
		}
		protected bool Equals(NLogInteractiveWrapper other)
		{
			return _logger.Equals(other._logger);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != this.GetType())
				return false;

			return Equals((NLogInteractiveWrapper)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return _logger.GetHashCode();
		}
	}
}