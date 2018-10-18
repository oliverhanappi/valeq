using System;
using System.Collections;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class EqualityComparerAttribute : Attribute, IEqualityComparerMetadata
    {
        public Type EqualityComparerType { get; }

        public EqualityComparerAttribute(Type equalityComparerType)
        {
            if (equalityComparerType == null)
                throw new ArgumentNullException(nameof(equalityComparerType));

            if (!typeof(IEqualityComparer).IsAssignableFrom(equalityComparerType))
                throw new ArgumentException($"{equalityComparerType.GetDisplayName()} is not an equality comparer.");

            EqualityComparerType = equalityComparerType;
        }

        public IEqualityComparer GetEqualityComparer(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return (IEqualityComparer) context.Configuration.Activator.CreateInstance(EqualityComparerType, context);
        }
    }
}
