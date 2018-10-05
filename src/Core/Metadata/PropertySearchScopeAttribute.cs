using System;
using Valeq.Configuration;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class PropertySearchScopeAttribute : Attribute, IPropertySearchScopeMetadata
    {
        public PropertySearchScope PropertySearchScope { get; }

        public PropertySearchScopeAttribute(PropertySearchScope propertySearchScope)
        {
            PropertySearchScope = propertySearchScope;
        }

        public PropertySearchScope GetPropertySearchScope(EqualityComparerContext context)
        {
            return PropertySearchScope;
        }
    }
}