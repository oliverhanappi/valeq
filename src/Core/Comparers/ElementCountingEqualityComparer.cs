using System;
using System.Collections.Generic;
using System.Linq;
using Valeq.Utils;

namespace Valeq.Comparers
{
    public abstract class ElementCountingEqualityComparer<T> : GenericEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> _elementEqualityComparer;
        private readonly Func<int, int> _countModifier;

        protected ElementCountingEqualityComparer(
            IEqualityComparer<T> elementEqualityComparer,
            Func<int, int> countModifier)
        {
            _elementEqualityComparer = elementEqualityComparer ??
                                       throw new ArgumentNullException(nameof(elementEqualityComparer));
            _countModifier = countModifier ?? throw new ArgumentNullException(nameof(countModifier));
        }

        protected override bool EqualsInternal(IEnumerable<T> x, IEnumerable<T> y)
        {
            var elementCountsX = CountElements(x);
            var elementCountsY = CountElements(y);

            return elementCountsX
                .FullOuterJoin(elementCountsY,
                    countX => countX.Element,
                    countY => countY.Element,
                    (_, countX, countY) => new {CountX = countX?.Count ?? 0, CountY = countY?.Count ?? 0},
                    keyComparer: _elementEqualityComparer)
                .All(counts => counts.CountX == counts.CountY);
        }

        protected override int GetHashCodeInternal(IEnumerable<T> obj)
        {
            unchecked
            {
                var hashCode = 0;

                foreach (var elementCount in CountElements(obj))
                {
                    hashCode = hashCode ^ elementCount.Count;
                    hashCode = hashCode ^ _elementEqualityComparer.GetHashCode(elementCount.Element);
                }

                return hashCode;
            }
        }

        private IEnumerable<ElementCount> CountElements(IEnumerable<T> enumerable)
        {
            return enumerable.GroupBy(
                e => e,
                (e, es) => new ElementCount(e, _countModifier(es.Count())),
                _elementEqualityComparer);
        }

        private class ElementCount
        {
            public T Element { get; }
            public int Count { get; }

            public ElementCount(T element, int count)
            {
                Element = element;
                Count = count;
            }
        }
    }
}
