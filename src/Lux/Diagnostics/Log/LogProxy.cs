using System;

namespace Lux.Diagnostics.Log
{
    public abstract class LogProxyBase : ILog
    {
        protected readonly ILog Log;

        protected LogProxyBase(ILog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            Log = log;
        }


        public virtual void Debug(object message)
        {
            Log?.Debug(message);
        }

        public virtual void Debug(object message, Exception exception)
        {
            Log?.Debug(message, exception);
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            Log?.DebugFormat(format, args);
        }

        public virtual void DebugFormat(string format, object arg0)
        {
            Log?.DebugFormat(format, arg0);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1)
        {
            Log?.DebugFormat(format, arg0, arg1);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Log?.DebugFormat(format, arg0, arg1, arg2);
        }

        public virtual void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log?.DebugFormat(provider, format, args);
        }

        public virtual void Info(object message)
        {
            Log?.Info(message);
        }

        public virtual void Info(object message, Exception exception)
        {
            Log?.Info(message, exception);
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            Log?.InfoFormat(format, args);
        }

        public virtual void InfoFormat(string format, object arg0)
        {
            Log?.InfoFormat(format, arg0);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1)
        {
            Log?.InfoFormat(format, arg0, arg1);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Log?.InfoFormat(format, arg0, arg1, arg2);
        }

        public virtual void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log?.InfoFormat(provider, format, args);
        }

        public virtual void Warn(object message)
        {
            Log?.Warn(message);
        }

        public virtual void Warn(object message, Exception exception)
        {
            Log?.Warn(message, exception);
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            Log?.WarnFormat(format, args);
        }

        public virtual void WarnFormat(string format, object arg0)
        {
            Log?.WarnFormat(format, arg0);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1)
        {
            Log?.WarnFormat(format, arg0, arg1);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Log?.WarnFormat(format, arg0, arg1, arg2);
        }

        public virtual void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log?.WarnFormat(provider, format, args);
        }

        public virtual void Error(object message)
        {
            Log?.Error(message);
        }

        public virtual void Error(object message, Exception exception)
        {
            Log?.Error(message, exception);
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            Log?.ErrorFormat(format, args);
        }

        public virtual void ErrorFormat(string format, object arg0)
        {
            Log?.ErrorFormat(format, arg0);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1)
        {
            Log?.ErrorFormat(format, arg0, arg1);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Log?.ErrorFormat(format, arg0, arg1, arg2);
        }

        public virtual void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log?.ErrorFormat(provider, format, args);
        }

        public virtual void Fatal(object message)
        {
            Log?.Fatal(message);
        }

        public virtual void Fatal(object message, Exception exception)
        {
            Log?.Fatal(message, exception);
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            Log?.FatalFormat(format, args);
        }

        public virtual void FatalFormat(string format, object arg0)
        {
            Log?.FatalFormat(format, arg0);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1)
        {
            Log?.FatalFormat(format, arg0, arg1);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Log?.FatalFormat(format, arg0, arg1, arg2);
        }

        public virtual void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log?.FatalFormat(provider, format, args);
        }

        public bool IsDebugEnabled => Log?.IsDebugEnabled ?? false;
        public bool IsInfoEnabled  => Log?.IsInfoEnabled ?? false;
        public bool IsWarnEnabled  => Log?.IsWarnEnabled ?? false;
        public bool IsErrorEnabled => Log?.IsErrorEnabled ?? false;
        public bool IsFatalEnabled => Log?.IsFatalEnabled ?? false;
    }
}
