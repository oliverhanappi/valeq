using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Valueq.Reflection
{
    public class FieldMemberProvider : IMemberProvider
    {
        public IEnumerable<Member> GetMembers(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(Member.FromFieldInfo);
        }
    }
}
