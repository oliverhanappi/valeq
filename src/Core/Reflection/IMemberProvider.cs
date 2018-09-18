using System;
using System.Collections.Generic;

namespace Valueq.Reflection
{
    public interface IMemberProvider
    {
        IEnumerable<Member> GetMembers(Type type);
    }
}
