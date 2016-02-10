using System;

namespace Lux.Diagnostics
{
    /// <summary>
    /// A copy of log4net's ILog interface
    /// </summary>
    public interface ILog
    {
        /// <summary>
		/// Checks if this logger is enabled for the "Debug" level.
		/// </summary>
		/// <value>
		/// <c>true</c> if this logger is enabled for "Debug" events, <c>false</c> otherwise.
		/// </value>
		/// <seealso cref="M:Debug(object)"/>
		/// <seealso cref="M:DebugFormat(IFormatProvider, string, object[])"/>
		bool IsDebugEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the "Info" level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this logger is enabled for "Info" events, <c>false</c> otherwise.
        /// </value>
        /// <seealso cref="M:Info(object)"/>
        /// <seealso cref="M:InfoFormat(IFormatProvider, string, object[])"/>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the "Warn" level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this logger is enabled for "Warn" events, <c>false</c> otherwise.
        /// </value>
        /// <seealso cref="M:Warn(object)"/>
        /// <seealso cref="M:WarnFormat(IFormatProvider, string, object[])"/>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the "Error" level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this logger is enabled for "Error" events, <c>false</c> otherwise.
        /// </value>
        /// <seealso cref="M:Error(object)"/>
        /// <seealso cref="M:ErrorFormat(IFormatProvider, string, object[])"/>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Checks if this logger is enabled for the "Fatal" level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this logger is enabled for "Fatal" events, <c>false</c> otherwise.
        /// </value>
        /// <seealso cref="M:Fatal(object)"/>
        /// <seealso cref="M:FatalFormat(IFormatProvider, string, object[])"/>
        bool IsFatalEnabled { get; }

        /// <overloads>Log a message object with the "Debug" level.</overloads>
        /// <summary>
        /// Log a message object with the "Debug" level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Debug(object message);

        /// <summary>
        /// Log a message object with the "Level.Debug" level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Debug(object message, Exception exception);

        /// <overloads>Log a formatted string with the "Debug" level.</overloads>
        /// <summary>
        /// Logs a formatted message string with the "Debug" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted message string with the "Debug" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        void DebugFormat(string format, object arg0);

        /// <summary>
        /// Logs a formatted message string with the "Debug" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        void DebugFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Logs a formatted message string with the "Debug" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        void DebugFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs a formatted message string with the "Debug" level.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <overloads>Log a message object with the "Info" level.</overloads>
        /// <summary>
        /// Logs a message object with the "Info" level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Info(object message);

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Info(object message, Exception exception);

        /// <overloads>Log a formatted message string with the "Info" level.</overloads>
        /// <summary>
        /// Logs a formatted message string with the "Info" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted message string with the "Info" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        void InfoFormat(string format, object arg0);

        /// <summary>
        /// Logs a formatted message string with the "Info" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        void InfoFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Logs a formatted message string with the "Info" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        void InfoFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs a formatted message string with the "Info" level.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <overloads>Log a message object with the "Warn" level.</overloads>
        /// <summary>
        /// Log a message object with the "Warn" level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(object message);

        /// <summary>
        /// Log a message object with the "Warn" level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Warn(object message, Exception exception);

        /// <overloads>Log a formatted message string with the "Warn" level.</overloads>
        /// <summary>
        /// Logs a formatted message string with the "Warn" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted message string with the "Warn" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        void WarnFormat(string format, object arg0);

        /// <summary>
        /// Logs a formatted message string with the "Warn" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        void WarnFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Logs a formatted message string with the "Warn" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        void WarnFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs a formatted message string with the "Warn" level.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void WarnFormat(IFormatProvider provider, string format, params object[] args);

        /// <overloads>Log a message object with the "Error" level.</overloads>
        /// <summary>
        /// Logs a message object with the "Error" level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Error(object message);

        /// <summary>
        /// Log a message object with the "Error" level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Error(object message, Exception exception);

        /// <overloads>Log a formatted message string with the "Error" level.</overloads>
        /// <summary>
        /// Logs a formatted message string with the "Error" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted message string with the "Error" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        void ErrorFormat(string format, object arg0);

        /// <summary>
        /// Logs a formatted message string with the "Error" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        void ErrorFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Logs a formatted message string with the "Error" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        void ErrorFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs a formatted message string with the "Error" level.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <overloads>Log a message object with the "Fatal" level.</overloads>
        /// <summary>
        /// Log a message object with the "Fatal" level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Fatal(object message);

        /// <summary>
        /// Log a message object with the "Fatal" level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Fatal(object message, Exception exception);

        /// <overloads>Log a formatted message string with the "Fatal" level.</overloads>
        /// <summary>
        /// Logs a formatted message string with the "Fatal" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted message string with the "Fatal" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        void FatalFormat(string format, object arg0);

        /// <summary>
        /// Logs a formatted message string with the "Fatal" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        void FatalFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Logs a formatted message string with the "Fatal" level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        void FatalFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs a formatted message string with the "Fatal" level.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
    }
}
