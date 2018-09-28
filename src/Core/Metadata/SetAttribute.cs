using System;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class SetAttribute : Attribute, ICollectionCategoryMetadata
    {
        public CollectionCategory GetCollectionCategory(EqualityComparerContext context)
        {
            return CollectionCategory.Set;
        }
    }
}
