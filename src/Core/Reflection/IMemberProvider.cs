using System;
using System.Collections.Generic;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public interface IMemberProvider
    {
        IEnumerable<Member> GetMembers(EqualityComparerContext context);
    }
}
