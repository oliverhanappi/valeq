using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Valeq.Reflection
{
    public static class MemberAccess
    {
        public static Func<object, object> CreateFieldGetter(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            var instanceParameter = Expression.Parameter(typeof(object));

            var castedInstance = Expression.Convert(instanceParameter, fieldInfo.DeclaringType);
            var fieldValue = Expression.Field(castedInstance, fieldInfo);
            var result = Expression.Convert(fieldValue, typeof(object));

            return (Func<object, object>) Expression.Lambda(result, instanceParameter).Compile();
        }

        public static Func<object, object> CreatePropertyGetter(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            if (propertyInfo.GetIndexParameters().Length > 0)
                throw new ArgumentException(
                    $"Indexed properties are not supported, however {propertyInfo.GetDisplayName()} is indexed.");

            var instanceParameter = Expression.Parameter(typeof(object));

            var castedInstance = Expression.Convert(instanceParameter, propertyInfo.DeclaringType);
            var fieldValue = Expression.Property(castedInstance, propertyInfo);
            var result = Expression.Convert(fieldValue, typeof(object));

            return (Func<object, object>) Expression.Lambda(result, instanceParameter).Compile();
        }
    }
}