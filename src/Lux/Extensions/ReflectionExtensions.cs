using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lux
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyFromExpression<T>(this Expression<Func<T, object>> propertyLambda)
        {
            MemberExpression expression = null;

            //this line is necessary, because sometimes the expression comes in as Convert(originalexpression)
            if (propertyLambda.Body is UnaryExpression)
            {
                var unary = (UnaryExpression)propertyLambda.Body;
                if (unary.Operand is MemberExpression)
                {
                    expression = (MemberExpression)unary.Operand;
                }
                else
                    throw new ArgumentException("Invalid property selector");
            }
            else if (propertyLambda.Body is MemberExpression)
            {
                expression = (MemberExpression)propertyLambda.Body;
            }
            else
            {
                throw new ArgumentException("Invalid property selector");
            }

            var propInfo = (PropertyInfo) expression.Member;
            return propInfo;
        }

    }
}
