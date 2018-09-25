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

            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(Member.FromPropertyInfo);
        }
    }
}