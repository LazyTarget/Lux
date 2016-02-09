using System;
using System.Linq.Expressions;
using System.Reflection;

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

    }
}
