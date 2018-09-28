using System.Collections;
using System.Collections.Generic;

namespace Valeq.Comparers
{
    public interface IGenericEqualityComparer<in T> : IEqualityComparer<T>, IEqualityComparer
    {
    }
}
