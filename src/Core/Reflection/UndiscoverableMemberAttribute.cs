using System;
using Valeq.Metadata;

namespace Valeq.Reflection
{
    /// <summary>
    /// Used to exclude members from being discovered by <see cref="FieldMemberProvider"/> and
    /// <see cref="PropertyMemberProvider"/>. The members are not taken into account when deciding
    /// whether value equality should be determined by fields or properties.
    /// Use with caution if the applied only to a field or a property but not both. It is
    /// recommended to use <see cref="IgnoreInComparisonAttribute"/> whenever possible. 
    /// </summary>
    [AttributeUsage(ValeqAttributeTargets.Member)]
    public class UndiscoverableMemberAttribute : Attribute
    {
    }
}
