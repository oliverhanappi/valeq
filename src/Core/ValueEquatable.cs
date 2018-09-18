using System;
using System.Collections;
using Valeq.Configuration;

namespace Valeq
{
    public abstract class ValueEquatable
    {
        public static bool operator ==(ValueEquatable x, ValueEquatable y) => Equals(x, y);
        public static bool operator !=(ValueEquatable x, ValueEquatable y) => !Equals(x, y);

        private readonly Lazy<IEqualityComparer> _equalityComparer;

        protected ValueEquatable()
        {
            _equalityComparer = new Lazy<IEqualityComparer>(GetEqualityComparer);
        }

        protected virtual IEqualityComparer GetEqualityComparer()
        {
            return ValueEqualityConfiguration.Current.ValueEqualityComparerProvider.GetEqualityComparer(GetType());
        }

        public override bool Equals(object obj)
        {
            return _equalityComparer.Value.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            return _equalityComparer.Value.GetHashCode(this);
        }
    }
}
