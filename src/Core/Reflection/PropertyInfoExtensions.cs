using System;
using System.Linq;
using System.Reflection;

namespace Valeq.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static PropertyInfo GetRootPropertyInfo(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            var comparisonMethod = GetComparisonMethod(propertyInfo);
            var baseMethod = comparisonMethod.GetBaseDefinition();

            if (baseMethod == comparisonMethod)
                return propertyInfo;

            var baseProperties = baseMethod.DeclaringType.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            return baseProperties
                .Where(p => p.Name == propertyInfo.Name)
                .Single(p => GetComparisonMethod(p) == baseMethod);

            MethodInfo GetComparisonMethod(PropertyInfo value) => value.CanRead ? value.GetMethod : value.SetMethod;
        }
    }
}
