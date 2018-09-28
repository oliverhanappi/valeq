using System;
using Valeq.Configuration;

namespace Valeq.Metadata
{
    /// <summary>
    /// Indicates that a type or member should be compared using default equality.
    /// Useful if the default equality comparison method is not configured to default equality.
    /// </summary>
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class DefaultEqualityAttribute : Attribute, IEqualityComparisonTypeMetadata
    {
        public EqualityComparisonType EqualityComparisonType => EqualityComparisonType.DefaultEquality;
    }
}
