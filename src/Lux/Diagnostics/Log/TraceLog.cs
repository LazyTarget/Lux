using System;

namespace Lux.Diagnostics
{
    public class TraceLog : ILog
    {
        public bool IsDebugEnabled => true;
        public bool IsInfoEnabled  => true;
        public bool IsWarnEnabled  => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;


        public virtual void Debug(object message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }

        public virtual void Debug(object message, Exception exception)
        {
            System.Diagnostics.Trace.WriteLine(message);
            System.Diagnostics.Trace.WriteLine(exception);
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            System.Diagnostics.Trace.WriteLine(message);
        }

        public virtual void DebugFormat(string format, object arg0)
        {
            var message = string.Format(format, new[] {arg0});
            System.Diagnostics.Trace.WriteLine(message);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1)
        {
            var message = string.Format(format, new[] { arg0, arg1 });
            System.Diagnostics.Trace.WriteLine(message);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            var message = string.Format(format, new[] { arg0, arg1, arg2 });
            System.Diagnostics.Trace.WriteLine(message);
        }

        public virtual void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            var message = string.Format(provider, format, args);
            System.Diagnostics.Trace.WriteLine(message);
        }

        public virtual void Info(object message)
        {
            System.Diagnostics.Trace.TraceInformation(message.ToString());
        }

        public virtual void Info(object message, Exception exception)
        {
            System.Diagnostics.Trace.TraceInformation(message.ToString());
            System.Diagnostics.Trace.TraceInformation(message.ToString());
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public virtual void InfoFormat(string format, object arg0)
        {
            var message = string.Format(format, new[] { arg0 });
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1)
        {
            var message = string.Format(format, new[] { arg0, arg1 });
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            var message = string.Format(format, new[] { arg0, arg1, arg2 });
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public virtual void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            var message = string.Format(provider, format, args);
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public virtual void Warn(object message)
        {
            System.Diagnostics.Trace.TraceWarning(message.ToString());
        }

        public virtual void Warn(object message, Exception exception)
        {
            System.Diagnostics.Trace.TraceWarning(message.ToString());
            System.Diagnostics.Trace.TraceWarning(message.ToString());
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public virtual void WarnFormat(string format, object arg0)
        {
            var message = string.Format(format, new[] { arg0 });
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1)
        {
            var message = string.Format(format, new[] { arg0, arg1 });
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            var message = string.Format(format, new[] { arg0, arg1, arg2 });
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public virtual void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            var message = string.Format(provider, format, args);
            System.Diagnostics.Trace.TraceWarning(message);
        }
        
        public virtual void Error(object message)
        {
            System.Diagnostics.Trace.TraceError(message.ToString());
        }

        public virtual void Error(object message, Exception exception)
        {
            System.Diagnostics.Trace.TraceError(message.ToString());
            System.Diagnostics.Trace.TraceError(message.ToString());
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void ErrorFormat(string format, object arg0)
        {
            var message = string.Format(format, new[] { arg0 });
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1)
        {
            var message = string.Format(format, new[] { arg0, arg1 });
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            var message = string.Format(format, new[] { arg0, arg1, arg2 });
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            var message = string.Format(provider, format, args);
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void Fatal(object message)
        {
            System.Diagnostics.Trace.TraceError(message.ToString());
        }

        public virtual void Fatal(object message, Exception exception)
        {
            System.Diagnostics.Trace.TraceError(message.ToString());
            System.Diagnostics.Trace.TraceError(message.ToString());
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void FatalFormat(string format, object arg0)
        {
            var message = string.Format(format, new[] { arg0 });
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1)
        {
            var message = string.Format(format, new[] { arg0, arg1 });
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            var message = string.Format(format, new[] { arg0, arg1, arg2 });
            System.Diagnostics.Trace.TraceError(message);
        }

        public virtual void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            var message = string.Format(provider, format, args);
            System.Diagnostics.Trace.TraceError(message);
        }
    }
}
