using System;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Diagnostics
{
    public class AggregateLog : ILog
    {
        private IList<ILog> _loggers;

        public AggregateLog()
        {
            _loggers = new List<ILog>();
        }

        public AggregateLog(params ILog[] loggers)
            : this()
        {
            if (loggers != null)
                _loggers = loggers.ToList();
        }


        public IList<ILog> Loggers
        {
            get { return _loggers; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _loggers = value;
            }
        }

        public IEnumerable<ILog> GetEnumerable()
        {
            var list = _loggers.ToList();
            return list.AsEnumerable();
        }


        public bool IsDebugEnabled
        {
            get
            {
                var res = GetEnumerable().Any(x => x.IsDebugEnabled);
                return res;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                var res = GetEnumerable().Any(x => x.IsInfoEnabled);
                return res;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                var res = GetEnumerable().Any(x => x.IsWarnEnabled);
                return res;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                var res = GetEnumerable().Any(x => x.IsErrorEnabled);
                return res;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                var res = GetEnumerable().Any(x => x.IsFatalEnabled);
                return res;
            }
        }
        

        public void Debug(object message)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Debug(message);
            }
        }

        public void Debug(object message, Exception exception)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Debug(message, exception);
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.DebugFormat(format, args);
            }
        }

        public void DebugFormat(string format, object arg0)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.DebugFormat(format, arg0);
            }
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.DebugFormat(format, arg0, arg1);
            }
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.DebugFormat(format, arg0, arg1, arg2);
            }
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.DebugFormat(provider, format, args);
            }
        }

        public void Info(object message)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Info(message);
            }
        }

        public void Info(object message, Exception exception)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Info(message, exception);
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.InfoFormat(format, args);
            }
        }

        public void InfoFormat(string format, object arg0)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.InfoFormat(format, arg0);
            }
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.InfoFormat(format, arg0, arg1);
            }
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.InfoFormat(format, arg0, arg1, arg2);
            }
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.InfoFormat(provider, format, args);
            }
        }

        public void Warn(object message)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Warn(message);
            }
        }

        public void Warn(object message, Exception exception)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Warn(message, exception);
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.WarnFormat(format, args);
            }
        }

        public void WarnFormat(string format, object arg0)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.WarnFormat(format, arg0);
            }
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.WarnFormat(format, arg0, arg1);
            }
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.WarnFormat(format, arg0, arg1, arg2);
            }
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.WarnFormat(provider, format, args);
            }
        }

        public void Error(object message)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Error(message);
            }
        }

        public void Error(object message, Exception exception)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Error(message, exception);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.ErrorFormat(format, args);
            }
        }

        public void ErrorFormat(string format, object arg0)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.ErrorFormat(format, arg0);
            }
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.ErrorFormat(format, arg0, arg1);
            }
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.ErrorFormat(format, arg0, arg1, arg2);
            }
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.ErrorFormat(provider, format, args);
            }
        }

        public void Fatal(object message)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Fatal(message);
            }
        }

        public void Fatal(object message, Exception exception)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.Fatal(message, exception);
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.FatalFormat(format, args);
            }
        }

        public void FatalFormat(string format, object arg0)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.FatalFormat(format, arg0);
            }
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.FatalFormat(format, arg0, arg1);
            }
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.FatalFormat(format, arg0, arg1, arg2);
            }
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            foreach (var logger in GetEnumerable())
            {
                logger.FatalFormat(provider, format, args);
            }
        }
    }
}
