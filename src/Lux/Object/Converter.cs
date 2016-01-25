using System;
using System.Globalization;
using System.Linq;
using Lux.Interfaces;

namespace Lux
{
    public class Converter : IConverter
    {
        private ITypeInstantiator _typeInstantiator;
        
        public Converter()
            : this(new TypeInstantiator())
        {
            
        }

        public Converter(ITypeInstantiator typeInstantiator)
        {
            _typeInstantiator = typeInstantiator;
        }

        public bool ThrowOnError { get; set; }

        public ITypeInstantiator TypeInstantiator
        {
            get { return _typeInstantiator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _typeInstantiator = value;
            }
        }


        public T Convert<T>(object value)
        {
            var result = ConvertWithDefault<T>(value, default(T));
            return result;
        }

        public T ConvertWithDefault<T>(object value, T defaultValue)
        {
            bool success;
            var result = ConvertWithDefault<T>(value, defaultValue, out success);
            return result;
        }

        public virtual T ConvertWithDefault<T>(object value, T defaultValue, out bool success)
        {
            success = true;
            var result = defaultValue;
            try
            {
                if (value == null)
                    return result;
                if (value == DBNull.Value)
                    return result;
                var type = value.GetType();
                if (typeof (T) == type)
                {
                    result = (T) value;
                }
                else if (typeof(T).IsEnum)
                {
                    var str = Convert<string>(value);
                    if (type == typeof (string))
                    {
                        var values = Enum.GetValues(typeof(T)).Cast<T>().Select(x => x.ToString()).ToList();
                        var index = values.FindIndex(x => string.Equals(x, str, StringComparison.OrdinalIgnoreCase));
                        if (index >= 0)
                            str = values.ElementAt(index);
                        else if (str.IndexOf('|') > 0)
                        {
                            str = str.Replace('|', ',');
                        }
                    }
                    var obj = Enum.Parse(typeof(T), str);
                    result = (T)obj;
                }
                else if (typeof(IConvertible).IsAssignableFrom(typeof (T)))
                {
                    if (type == typeof (string))
                    {
                        var str = Convert<string>(value) ?? "";
                        var isNumeric = str.Length > 0 && str.All(c => char.IsDigit(c) || char.IsPunctuation(c) || c == ',' || c == '+');
                        if (isNumeric)
                            value = str.Replace(',', '.');
                    }
                    var obj = System.Convert.ChangeType(value, typeof (T), CultureInfo.InvariantCulture);
                    result = (T) obj;
                }
                else if (typeof(TimeSpan) == typeof(T))
                {
                    var str = Convert<string>(value) ?? "";
                    object timeSpan = TimeSpan.Parse(str);
                    result = (T)timeSpan;
                }
                else if (typeof(Uri) == typeof(T))
                {
                    var str = Convert<string>(value) ?? "";
                    object uri = new Uri(str);
                    result = (T)uri;
                }
                else if (typeof(Guid) == typeof(T))
                {
                    var str = Convert<string>(value) ?? "";
                    object guid = new Guid(str);
                    result = (T) guid;
                }
                else
                {
                    result = (T) value;
                }
                return result;
            }
            catch (Exception ex)
            {
                if (ThrowOnError)
                    throw;
                success = false;
                return result;
            }
        }


        public virtual object Convert(object value, Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            object result = null;
            try
            {
                //var defaultValue = targetType.IsInterface
                //    ? null
                //    : Activator.CreateInstance(targetType);
                var defaultValue = targetType.IsInterface
                    ? null
                    : _typeInstantiator.Instantiate(targetType);
                result = ConvertWithDefault(value, targetType, defaultValue);
                return result;
            }
            catch (Exception ex)
            {
                if (ThrowOnError)
                    throw;
                return result;
            }
        }

        public object ConvertWithDefault(object value, Type targetType, object defaultValue)
        {
            bool success;
            var result = ConvertWithDefault(value, targetType, defaultValue, out success);
            return result;
        }

        public virtual object ConvertWithDefault(object value, Type targetType, object defaultValue, out bool success)
        {
            success = true;
            var result = defaultValue;
            try
            {
                if (value == null)
                    return result;
                if (value == DBNull.Value)
                    return result;
                var type = value.GetType();
                if (targetType == type)
                {
                    result = value;
                }
                else if (targetType.IsAssignableFrom(type))
                {
                    result = value;
                }
                else if (targetType.IsEnum)
                {
                    var str = Convert<string>(value);
                    if (type == typeof (string))
                    {
                        var values = Enum.GetValues(targetType).Cast<Enum>().Select(x => x.ToString()).ToList();
                        var index = values.FindIndex(x => string.Equals(x, str, StringComparison.OrdinalIgnoreCase));
                        if (index >= 0)
                            str = values.ElementAt(index);
                    }
                    result = Enum.Parse(targetType, str);
                }
                else if (typeof(IConvertible).IsAssignableFrom(targetType))
                {
                    if (type == typeof (string))
                    {
                        var str = Convert<string>(value) ?? "";
                        var isNumeric = str.Length > 0 && str.All(c => char.IsDigit(c) || char.IsPunctuation(c) || c == ',' || c == '+');
                        if (isNumeric)
                            value = str.Replace(',', '.');
                    }
                    result = System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                }
                else if (typeof(TimeSpan) == targetType)
                {
                    var str = Convert<string>(value) ?? "";
                    object timeSpan = TimeSpan.Parse(str);
                    result = timeSpan;
                }
                else if (typeof(Uri) == targetType)
                {
                    var str = Convert<string>(value) ?? "";
                    object uri = new Uri(str);
                    result = uri;
                }
                else if (typeof(Guid) == targetType)
                {
                    var str = Convert<string>(value) ?? "";
                    object guid = new Guid(str);
                    result = guid;
                }
                else
                {
                    //result = (T) value;       // todo?
                    throw new NotImplementedException();
                }
                return result;
            }
            catch (Exception ex)
            {
                if (ThrowOnError)
                    throw;
                success = false;
                return result;
            }
        }

    }
}
