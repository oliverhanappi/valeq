using System;
using Valeq.Metadata;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Configuration
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
