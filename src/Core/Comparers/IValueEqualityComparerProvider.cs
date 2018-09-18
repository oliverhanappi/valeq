using System;
using System.Collections;

namespace Valueq.Comparers
{
    public interface IValueEqualityComparerProvider
    {
        IEqualityComparer GetEqualityComparer(Type type);
    }
}
