using System;
using System.Linq.Expressions;
using System.Reflection;
using Lux.Extensions;
using Lux.Interfaces;

namespace Lux.Model
{
    public class MirrorProperty : IProperty
    {
        private readonly object _instance;
        private readonly PropertyInfo _propertyInfo;
        private readonly IConverter _converter;

        public MirrorProperty(object instance, PropertyInfo propertyInfo)
            : this(instance, propertyInfo, new Converter())
        {
        }

        public MirrorProperty(object instance, PropertyInfo propertyInfo, IConverter converter)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            _instance = instance;
            _propertyInfo = propertyInfo;
            _converter = converter;
        }


        public string Name      => _propertyInfo.Name;
        public Type Type        => _propertyInfo.PropertyType;
        public bool ReadOnly    => !_propertyInfo.CanWrite;
        public object Value     => _propertyInfo.GetValue(_instance);


        public virtual void SetValue(object value)
        {
            if (_converter != null)
            {
                value = _converter.Convert(value, Type);
            }
            _propertyInfo.SetValue(_instance, value);
        }




        public static MirrorProperty Create(object instance, string propertyName)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName);
            var prop = Create(instance, propertyInfo);
            return prop;
        }

        public static MirrorProperty Create(object instance, string propertyName, IConverter converter)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName);
            var prop = Create(instance, propertyInfo, converter);
            return prop;
        }
        
        public static MirrorProperty Create(object instance, PropertyInfo propertyInfo)
        {
            var prop = new MirrorProperty(instance, propertyInfo);
            return prop;
        }

        public static MirrorProperty Create(object instance, PropertyInfo propertyInfo, IConverter converter)
        {
            var prop = new MirrorProperty(instance, propertyInfo, converter);
            return prop;
        }

        public static MirrorProperty Create<T>(T instance, Expression<Func<T, object>> propertyLambda)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var propertyInfo = propertyLambda.GetPropertyInfoByExpression();
            var prop = Create(instance, propertyInfo);
            return prop;
        }

        public static MirrorProperty Create<T>(T instance, Expression<Func<T, object>> propertyLambda, IConverter converter)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var propertyInfo = propertyLambda.GetPropertyInfoByExpression();
            var prop = Create(instance, propertyInfo, converter);
            return prop;
        }

    }
}
