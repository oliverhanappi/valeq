using System;
using System.Collections;
using Valeq.Comparers;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    /// <summary>
    /// The type's own Equals and GetHashCode implementations should be used for determining value equality. 
    /// </summary>
    [AttributeUsage(ValeqAttributeTargets.Type)]
    public class CustomEqualityAttribute : Attribute, IEqualityComparerMetadata
    {
        public IEqualityComparer GetEqualityComparer(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return DefaultEqualityComparer.GetForType(context.Scope.TargetType);
        }
    }
}
