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

            if (memberInfo.DeclaringType == type)
                return true;

            if (type.BaseType == null)
                return false;

            return memberInfo.IsPartOf(type.BaseType);
        }
    }
}
