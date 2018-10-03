using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Valeq.Reflection
{
    public class FieldMemberProvider : IMemberProvider
    {
        public IEnumerable<Member> GetMembers(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetBaseTypesAndSelf()
                .SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly |
                                             BindingFlags.Public | BindingFlags.NonPublic))
                .Where(f => f.GetCustomAttribute<UndiscoverableMemberAttribute>() == null)
                .Select(Member.FromFieldInfo);
        }
    }
}
