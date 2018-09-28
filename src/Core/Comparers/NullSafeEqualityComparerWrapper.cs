using System;
using System.Collections.Generic;

namespace Valeq.Comparers
{
    public class NullSafeEqualityComparerWrapper<T> : GenericEqualityComparer<T>
    {
        private readonly IEqualityComparer<T> _inner;

        public NullSafeEqualityComparerWrapper(IEqualityComparer<T> inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }
        
        protected override bool EqualsInternal(T x, T y)
        {
            return _inner.Equals(x, y);
        }

        protected override int GetHashCodeInternal(T obj)
        {
            return _inner.GetHashCode(obj);
        }
    }
}
