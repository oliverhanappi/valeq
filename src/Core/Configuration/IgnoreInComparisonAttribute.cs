using System;
using Valeq.Metadata;

namespace Valeq.Configuration
{
    [AttributeUsage(ValeqAttributeTargets.Member)]
    public class IgnoreInComparisonAttribute : Attribute, IIgnoredMemberMetadata
    {
    }
}
