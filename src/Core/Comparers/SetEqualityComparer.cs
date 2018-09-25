using System;
using System.Collections;
using System.Collections.Generic;
using Valeq.Reflection;

namespace Valeq.Comparers
{
    public static class SetEqualityComparer
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

            var genericType = typeof(SetEqualityComparer<>).MakeGenericType(elementType);
            return (IEqualityComparer) Activator.CreateInstance(genericType, elementEqualityComparer);
        }
    }
    
    public class SetEqualityComparer<T> : ElementCountingEqualityComparer<T>
    {
        public SetEqualityComparer(IEqualityComparer<T> elementEqualityComparer)
            : base(elementEqualityComparer, Math.Sign)
        {
        }
    }
}
