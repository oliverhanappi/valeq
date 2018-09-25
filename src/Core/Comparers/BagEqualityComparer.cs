using System;
using System.Collections;
using System.Collections.Generic;
using Valeq.Reflection;

namespace Valeq.Comparers
{
    public static class BagEqualityComparer
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

            var genericType = typeof(BagEqualityComparer<>).MakeGenericType(elementType);
            return (IEqualityComparer) Activator.CreateInstance(genericType, elementEqualityComparer);
        }
    }
    
    public class BagEqualityComparer<T> : ElementCountingEqualityComparer<T>
    {
        public BagEqualityComparer(IEqualityComparer<T> elementEqualityComparer)
            : base(elementEqualityComparer, c => c)
        {
        }
    }
}
