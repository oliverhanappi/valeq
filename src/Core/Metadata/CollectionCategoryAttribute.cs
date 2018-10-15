using System;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class CollectionCategoryAttribute : Attribute, ICollectionCategoryMetadata
    {
        public CollectionCategory CollectionCategory { get; }

        public CollectionCategoryAttribute(CollectionCategory collectionCategory)
        {
            CollectionCategory = collectionCategory;
        }

        public CollectionCategory GetCollectionCategory(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.Scope.TargetType.GetCategory() != TypeCategory.Collection)
            {
                var message = $"Target type {context.Scope.TargetType.GetDisplayName()} is not a collection.";
                throw new ArgumentException(message, nameof(context));
            }

            return CollectionCategory;
        }
    }
}
