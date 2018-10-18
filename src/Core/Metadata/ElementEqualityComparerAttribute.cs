using System;
using System.Collections;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.TypeOrMember)]
    public class ElementEqualityComparerAttribute : Attribute, IElementEqualityComparerMetadata
    {
        public Type ElementEqualityComparerType { get; }

        public ElementEqualityComparerAttribute(Type equalityComparerType)
        {
            if (equalityComparerType == null)
                throw new ArgumentNullException(nameof(equalityComparerType));

            if (!typeof(IEqualityComparer).IsAssignableFrom(equalityComparerType))
                throw new ArgumentException($"{equalityComparerType.GetDisplayName()} is not an equality comparer.");

            ElementEqualityComparerType = equalityComparerType;
        }

        public IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return (IEqualityComparer) context.Configuration.Activator
                .CreateInstance(ElementEqualityComparerType, context);
        }
    }
}
