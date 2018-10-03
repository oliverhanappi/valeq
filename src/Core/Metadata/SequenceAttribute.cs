using System;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class SequenceAttribute : Attribute, ICollectionCategoryMetadata
    {
        public CollectionCategory GetCollectionCategory(EqualityComparerContext context)
        {
            return CollectionCategory.Sequence;
        }
    }
}