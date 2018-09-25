using System.Runtime.CompilerServices;

namespace Valeq.Comparers
{
    public class ReferenceEqualityComparer<T> : GenericEqualityComparer<T>
    {
        protected override bool EqualsInternal(T x, T y)
        {
            // reference equality is already handled in GenericEqualityComparer<T>
            return false;
        }

        protected override int GetHashCodeInternal(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
