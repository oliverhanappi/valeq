using System;
using System.Collections.Generic;

namespace Valeq.Reflection
{
    public interface IMemberProvider
    {
        IEnumerable<Member> GetMembers(Type type);
    }
}
