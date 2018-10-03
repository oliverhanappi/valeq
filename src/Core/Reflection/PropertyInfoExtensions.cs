using System;
using System.Reflection;

namespace Valeq.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static bool IsOverride(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            return propertyInfo.CanRead
                ? propertyInfo.GetMethod.GetBaseDefinition() != propertyInfo.GetMethod
                : propertyInfo.SetMethod.GetBaseDefinition() != propertyInfo.SetMethod;
        }
    }
}
