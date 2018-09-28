using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Valeq.Reflection
{
    public static class ExpressionUtility
    {
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (!(expression.Body is MemberExpression memberExpression))
                throw new ArgumentException($"Expression {expression} does not refer to a member.");

            if (!(memberExpression.Member is PropertyInfo propertyInfo))
                throw new ArgumentException($"Expression {expression} does not refer to a property, " +
                                            $"but to {memberExpression.Member}");

            return propertyInfo;
        }
    }
}
