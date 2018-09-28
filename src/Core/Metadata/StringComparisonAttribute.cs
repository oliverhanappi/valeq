using System;
using System.Collections;
using Valeq.Comparers;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.Member)]
    public class StringComparisonAttribute : Attribute, IEqualityComparerMetadata
    {
        public StringComparison StringComparison { get; }

        public StringComparisonAttribute(StringComparison stringComparison)
        {
            StringComparison = stringComparison;
        }

        public IEqualityComparer GetEqualityComparer(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.Scope.TargetType != typeof(string))
                throw new ArgumentException($"{context} does not target a string.", nameof(context));

            return StringEqualityComparer.Get(StringComparison);
        }
    }
}
