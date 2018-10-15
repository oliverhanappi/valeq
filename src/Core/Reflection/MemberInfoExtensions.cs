using System;
using System.Reflection;

namespace Valeq.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool IsPartOf(this MemberInfo memberInfo, Type type)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (memberInfo is PropertyInfo propertyInfo)
                memberInfo = propertyInfo.GetRootPropertyInfo();

            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsAssignableFrom(type))
                return true;

            return false;
        }
    }
}
