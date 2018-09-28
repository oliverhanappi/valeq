using System;

namespace Valeq.Metadata
{
    public static class ValeqAttributeTargets
    {
        public const AttributeTargets Member = AttributeTargets.Field | AttributeTargets.Property;

        public const AttributeTargets Type = AttributeTargets.Class |
                                             AttributeTargets.Struct |
                                             AttributeTargets.Enum |
                                             AttributeTargets.Interface;

        public const AttributeTargets TypeOrMember = Type | Member;
    }
}
