using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public class FieldMemberProvider : IMemberProvider
    {
        public IEnumerable<Member> GetMembers(EqualityComparerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.Scope.TargetType.GetBaseTypes()
                .SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly |
                                             BindingFlags.Public | BindingFlags.NonPublic))
                .Where(f => f.GetCustomAttribute<UndiscoverableMemberAttribute>() == null)
                .Select(Member.FromFieldInfo);
        }
    }
}
