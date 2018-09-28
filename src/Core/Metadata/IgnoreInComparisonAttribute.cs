using System;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.Member)]
    public class IgnoreInComparisonAttribute : Attribute, IIgnoredMemberMetadata
    {
    }
}
