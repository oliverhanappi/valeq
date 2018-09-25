using System;
using System.Collections;

namespace Valeq.Runtime
{
    public interface IValueEqualityComparerProvider
    {
        IEqualityComparer GetEqualityComparer(Type type);
    }
}
