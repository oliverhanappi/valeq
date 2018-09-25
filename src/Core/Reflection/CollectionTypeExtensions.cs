using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Valeq.Reflection
{
    public static class CollectionTypeExtensions
    {
        public static CollectionCategory GetCollectionCategory(this Type collectionType)
        {
            if (collectionType == null)
                throw new ArgumentNullException(nameof(collectionType));
            
            if (collectionType.GetCategory() != TypeCategory.Collection)
                throw new ArgumentException($"{collectionType.GetDisplayName()} is not a collection.");

            if (collectionType.IsAssignableToAny(typeof(IReadOnlyList<>), typeof(IList<>), typeof(IList)))
                return CollectionCategory.Sequence;

            if (collectionType.IsAssignableTo(typeof(ISet<>)))
                return CollectionCategory.Set;

            if (collectionType.IsAssignableToAny(typeof(IReadOnlyCollection<>), typeof(ICollection<>), typeof(ICollection)))
                return CollectionCategory.Bag;

            return CollectionCategory.Sequence;
        }
        
        public static Type GetCollectionElementType(this Type collectionType)
        {
            if (collectionType == null)
                throw new ArgumentNullException(nameof(collectionType));
            
            if (collectionType.GetCategory() != TypeCategory.Collection)
                throw new ArgumentException($"{collectionType.GetDisplayName()} is not a collection.");

            if (collectionType.IsAssignableTo(typeof(IEnumerable<>)))
                return collectionType.GetGenericTypeParameters(typeof(IEnumerable<>)).Single();

            return typeof(object);
        }
    }
}
