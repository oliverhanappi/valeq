using System;
using System.Collections;
using System.Collections.Generic;
using Valeq.Reflection;

namespace Valeq.Comparers
{
    public static class NullableEqualityComparer
    {
        public static IEqualityComparer Create(Type underlyingType, IEqualityComparer underlyingEqualityComparer)
        {
            if (underlyingType == null) throw new ArgumentNullException(nameof(underlyingType));
            if (underlyingEqualityComparer == null) throw new ArgumentNullException(nameof(underlyingEqualityComparer));

            if (!underlyingType.IsValueType)
            {
                var message = $"{underlyingType.GetDisplayName()} is not a value type.";
                throw new ArgumentException(message, nameof(underlyingType));
            }

            var expectedEqualityComparerType = typeof(IEqualityComparer<>).MakeGenericType(underlyingType);
            if (!underlyingEqualityComparer.GetType().IsAssignableTo(expectedEqualityComparerType))
            {
                var message =
                    $"{underlyingEqualityComparer} is not of type {expectedEqualityComparerType.GetDisplayName()}";
                throw new ArgumentException(message, nameof(underlyingEqualityComparer));
            }

            var genericType = typeof(NullableEqualityComparer<>).MakeGenericType(underlyingType);
            return (IEqualityComparer) Activator.CreateInstance(genericType, underlyingEqualityComparer);
        }
    }

    public class NullableEqualityComparer<T> : GenericEqualityComparer<T?>
        where T : struct
    {
        private readonly IEqualityComparer<T> _underlyingEqualityComparer;

        public NullableEqualityComparer(IEqualityComparer<T> underlyingEqualityComparer)
        {
            _underlyingEqualityComparer = underlyingEqualityComparer
                                          ?? throw new ArgumentNullException(nameof(underlyingEqualityComparer));
        }

        protected override bool EqualsInternal(T? x, T? y)
        {
            if (!x.HasValue && !y.HasValue) return true;
            if (!x.HasValue || !y.HasValue) return false;

            return _underlyingEqualityComparer.Equals(x.Value, y.Value);
        }

        protected override int GetHashCodeInternal(T? obj)
        {
            return obj.HasValue ? _underlyingEqualityComparer.GetHashCode(obj.Value) : 0;
        }
    }
}
