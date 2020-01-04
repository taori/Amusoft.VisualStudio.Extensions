using System;

namespace Company.Desktop.Framework.Logging
{
	public interface IInteractiveLogger
	{
		void Error(Exception exception);
		void Error(string message);
		void Debug(string message);
		void Warning(string message);
		void Information(string message);
		void Fatal(Exception exception);
		void Fatal(string message);
		void Trace(string message);
	}
}