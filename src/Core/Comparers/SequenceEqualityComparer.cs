using System;
using System.Collections;
using System.Collections.Generic;
using Valeq.Reflection;
using Valeq.Utils;

namespace Valeq.Comparers
{
    public static class SequenceEqualityComparer
    {
        public static IEqualityComparer Create(Type elementType, IEqualityComparer elementEqualityComparer)
        {
            if (elementType == null) throw new ArgumentNullException(nameof(elementType));
            if (elementEqualityComparer == null) throw new ArgumentNullException(nameof(elementEqualityComparer));

            var expectedElementEqualityComparerType = typeof(IEqualityComparer<>).MakeGenericType(elementType);
            if (!elementEqualityComparer.GetType().IsAssignableTo(expectedElementEqualityComparerType))
                throw new ArgumentException(
                    $"{elementEqualityComparer} does not implement {expectedElementEqualityComparerType.GetDisplayName()}",
                    nameof(elementEqualityComparer));

            var genericType = typeof(SequenceEqualityComparer<>).MakeGenericType(elementType);
            return (IEqualityComparer) Activator.CreateInstance(genericType, elementEqualityComparer);
        }
    }
    
    public class SequenceEqualityComparer<T> : GenericEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> _elementEqualityComparer;

        public SequenceEqualityComparer(IEqualityComparer<T> elementEqualityComparer)
        {
            _elementEqualityComparer = elementEqualityComparer ??
                                       throw new ArgumentNullException(nameof(elementEqualityComparer));
        }

        protected override bool EqualsInternal(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (x.TryGetCount(out var countX) && y.TryGetCount(out var countY) && countX != countY)
                return false;

            using (var enumeratorX = x.GetEnumerator())
            using (var enumeratorY = y.GetEnumerator())
            {
                while (true)
                {
                    var moveNextX = enumeratorX.MoveNext();
                    var moveNextY = enumeratorY.MoveNext();

                    if (!moveNextX && !moveNextY)
                        return true;

                    if (!moveNextX || !moveNextY)
                        return false;

                    if (!_elementEqualityComparer.Equals(enumeratorX.Current, enumeratorY.Current))
                        return false;
                }
            }
        }

        protected override int GetHashCodeInternal(IEnumerable<T> obj)
        {
            unchecked
            {
                var hashCode = 0;

                foreach (var element in obj)
                {
                    hashCode = (hashCode * 397) ^ _elementEqualityComparer.GetHashCode(element);
                }

                return hashCode;
            }
        }
    }
}
