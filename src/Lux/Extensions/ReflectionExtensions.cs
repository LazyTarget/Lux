using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lux.Extensions
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyInfoByExpression<TObj, TVal>(this Expression<Func<TObj, TVal>> expression)
        {
            MemberExpression memberExpression;

            //this line is necessary, because sometimes the expression comes in as Convert(originalexpression)
            if (expression.Body is UnaryExpression)
            {
                var unary = (UnaryExpression)expression.Body;
                if (unary.Operand is MemberExpression)
                {
                    memberExpression = (MemberExpression)unary.Operand;
                }
                else
                    throw new ArgumentException("Invalid property selector");
            }
            else if (expression.Body is MemberExpression)
            {
                memberExpression = (MemberExpression)expression.Body;
            }
            else
            {
                throw new ArgumentException("Invalid property selector");
            }

            var propInfo = (PropertyInfo) memberExpression.Member;
            return propInfo;
        }

        
        public static TValue TryGetValueByObjectExpression<TObj, TValue>(this TObj obj, Expression<Func<TObj, TValue>> expression)
        {
            TValue value;
            try
            {
                PropertyInfo propInfo = null;
                //propInfo = GetPropertyInfoByExpression(expression);
                if (propInfo != null)
                {
                    var o = propInfo.GetValue(obj);
                    value = (TValue) o;
                }
                else
                {
                    value = expression.Compile().Invoke(obj);
                }
            }
            catch (Exception ex)
            {
                value = default(TValue);
            }
            return value;
        }


        /// <summary>
        /// Checks a type to see if it derives from a raw generic (e.g. List[[]])
        /// </summary>
        /// <param name="toCheck"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType
                    ? toCheck.GetGenericTypeDefinition()
                    : toCheck;

                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }
            return false;
        }


        /// <summary>
        /// Find a value from a System.Enum by trying several possible variants
        /// of the string value of the enum.
        /// </summary>
        /// <param name="type">Type of enum</param>
        /// <param name="value">Value for which to search</param>
        /// <param name="culture">The culture used to calculate the name variants</param>
        /// <returns></returns>
        public static object FindEnumValue(this Type type, string value, CultureInfo culture)
        {
            Enum ret = Enum.GetValues(type)
                           .Cast<Enum>()
                           .FirstOrDefault(v => v.ToString()
                                                 .GetNameVariants(culture)
                                                 .Contains(value, StringComparer.Create(culture, true)));

            if (ret == null)
            {
                object enumValueAsUnderlyingType = Convert.ChangeType(value, Enum.GetUnderlyingType(type), culture);

                if (enumValueAsUnderlyingType != null && Enum.IsDefined(type, enumValueAsUnderlyingType))
                {
                    ret = (Enum) Enum.ToObject(type, enumValueAsUnderlyingType);
                }
            }
            return ret;
        }



        public static IEnumerable<Type> GetDerivedTypes(this Type type, bool includeNonPublic = false)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var alltypes = assemblies.SelectMany(x =>
            {
                var res = includeNonPublic
                    ? x.GetTypes()
                    : x.GetExportedTypes();
                return res;
            });
            foreach (var t in alltypes)
            {
                if (t == type)
                    continue;
                var res = type.IsAssignableFrom(t);
                if (res)
                {
                    yield return t;
                }
            }
        }


        /// <summary>
        /// Retrieve an attribute from a member (property)
        /// </summary>
        /// <typeparam name="T">Type of attribute to retrieve</typeparam>
        /// <param name="prop">Member to retrieve attribute from</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this MemberInfo prop)
            where T : Attribute
        {
            var res = Attribute.GetCustomAttribute(prop, typeof(T)) as T;
            return res;
        }

        /// <summary>
        /// Retrieve an attribute from a type
        /// </summary>
        /// <typeparam name="T">Type of attribute to retrieve</typeparam>
        /// <param name="type">Type to retrieve attribute from</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type)
            where T : Attribute
        {
            var res = Attribute.GetCustomAttribute(type, typeof(T)) as T;
            return res;
        }


        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            var res = type.IsGenericType
                      && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic
                      &&
                      (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) ||
                       type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase))
                      && (type.Name.Contains("AnonymousType") || type.Name.Contains("AnonType"))
                      && Attribute.IsDefined(type, typeof (CompilerGeneratedAttribute), false);
            return res;
        }

    }
}
