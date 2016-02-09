using System;
using System.Linq.Expressions;
using Lux.Extensions;

namespace Lux.Serialization
{
    public static class ModelExtensions
    {
        //public static IProperty CreateMirrorProperty(this object obj, string propertyName)
        //{
        //    var prop = MirrorProperty.Create(obj, propertyName);
        //    return prop;
        //}

        //public static IProperty CreateMirrorProperty<T>(this T obj, Expression<Func<T, object>> propertyLambda)
        //{
        //    var prop = MirrorProperty.Create(obj, propertyLambda);
        //    return prop;
        //}


        public static T GetPropertyValue<T>(this T obj, Expression<Func<T, object>> propertyLambda)
            where T : IObject
        {
            var res = default(T);
            var propertyInfo = propertyLambda.GetPropertyInfoByExpression();
            if (propertyInfo != null)
            {
                var val = GetPropertyValue(obj, propertyInfo.Name);
                res = (T) val;
            }
            return res;
        }

        public static object GetPropertyValue(this IObject obj, string propertyName)
        {
            object res = null;
            var prop = obj?.GetProperty(propertyName);
            if (prop != null)
                res = prop.Value;
            return res;
        }

    }
}
