using System;
using System.Collections;

namespace Valeq.Comparers
{
    public interface IValueEqualityComparerProvider
    {
        IEqualityComparer GetEqualityComparer(Type type);
    }
}
