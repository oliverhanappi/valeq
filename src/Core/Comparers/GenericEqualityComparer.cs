using System.Collections;
using System.Collections.Generic;

namespace Valueq.Comparers
{
    public abstract class GenericEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return EqualsInternal(x, y);
        }

        protected abstract bool EqualsInternal(T x, T y);

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            if (x is T typedValueX)
            {
                if (y is T typedValueY)
                {
                    return EqualsInternal(typedValueX, typedValueY);
                }
                else
                {
                    // fallback to default equality (as implemented by framework comparers)
                    return y.Equals(x);
                }
            }
            else
            {
                // fallback to default equality (as implemented by framework comparers)
                return x.Equals(y);
            }
        }

        public int GetHashCode(T obj)
        {
            if (ReferenceEquals(obj, null))
                return 0;

            return GetHashCodeInternal(obj);
        }

        protected abstract int GetHashCodeInternal(T obj);

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (ReferenceEquals(obj, null))
                return 0;

            if (obj is T typedValue)
                return GetHashCodeInternal(typedValue);

            // fallback to default hashcode (as implemented by framework comparers)
            return obj.GetHashCode();
        }
    }
}
