using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Valeq.Reflection
{
    public class PropertyMemberProvider : IMemberProvider
    {
        public IEnumerable<Member> GetMembers(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetBaseTypesAndSelf()
                .SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly |
                                                 BindingFlags.Public | BindingFlags.NonPublic))
                .Where(p => p.GetCustomAttribute<UndiscoverableMemberAttribute>() == null)
                //TODO .Where(p => !p.IsOverride())
                .Select(Member.FromPropertyInfo);
        }
    }
}
