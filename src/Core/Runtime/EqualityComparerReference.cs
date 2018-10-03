using System;
using System.Collections;

namespace Valeq.Runtime
{
    public class EqualityComparerReference : IEqualityComparer
    {
        private readonly Lazy<IEqualityComparer> _equalityComparer;
        
        public EqualityComparerReference(Func<IEqualityComparer> provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _equalityComparer = new Lazy<IEqualityComparer>(provider);
        }
        
        public new bool Equals(object x, object y)
        {
            return _equalityComparer.Value.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return _equalityComparer.Value.GetHashCode(obj);
        }
    }
}
