using System;
using Valeq.Configuration;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    /// <summary>
    /// Indicates that a type or member should be compared using default equality.
    /// Useful if the default equality comparison method is not configured to default equality.
    /// </summary>
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class DefaultEqualityAttribute : Attribute, IEqualityComparisonTypeMetadata
    {
        public EqualityComparisonType GetEqualityComparisonType(EqualityComparerContext context)
        {
            return EqualityComparisonType.DefaultEquality;
        }
    }
}
