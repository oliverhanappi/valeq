using System;
using Valeq.Configuration;

namespace Valeq.Metadata
{
    /// <summary>
    /// Indicates that a type or member should be compared using value equality.
    /// Useful if the default equality comparison method is not configured to value equality.
    /// </summary>
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class ValueEqualityAttribute : Attribute, IEqualityComparisonTypeMetadata
    {
        public EqualityComparisonType EqualityComparisonType => EqualityComparisonType.ValueEquality;
    }
}
