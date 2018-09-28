using System;
using System.Collections.Generic;

namespace Valeq.Runtime
{
    public static class ValueEqualityComparerProviderExtensions
    {
        public static IEqualityComparer<T> GetEqualityComparer<T>(
            this IValueEqualityComparerProvider valueEqualityComparerProvider)
        {
            if (valueEqualityComparerProvider == null)
                throw new ArgumentNullException(nameof(valueEqualityComparerProvider));

            return (IEqualityComparer<T>) valueEqualityComparerProvider.GetEqualityComparer(typeof(T));
        }
    }
}
