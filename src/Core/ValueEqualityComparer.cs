using System;
using System.Collections;
using Valueq.Configuration;

namespace Valueq
{
    public class ValueEqualityComparer : IEqualityComparer
    {
        public ValueEqualityConfiguration Configuration { get; }

        public ValueEqualityComparer()
            : this(ValueEqualityConfiguration.Current)
        {
        }

        public ValueEqualityComparer(ValueEqualityConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            var equalityComparer = Configuration.ValueEqualityComparerProvider.GetEqualityComparer(x.GetType());
            return equalityComparer.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            if (ReferenceEquals(obj, null))
                return 0;

            var equalityComparer = Configuration.ValueEqualityComparerProvider.GetEqualityComparer(obj.GetType());
            return equalityComparer.GetHashCode(obj);
        }
    }
}
