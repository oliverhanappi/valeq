using System;
using System.Collections;
using Valeq.Comparers;
using Valeq.Configuration;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [AttributeUsage(ValeqAttributeTargets.Member)]
    public class IgnoreCaseInComparisonAttribute : Attribute, IEqualityComparerMetadata
    {
        public IEqualityComparer GetEqualityComparer(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.Scope.TargetType != typeof(string))
                throw new ArgumentException($"{context} does not target a string.", nameof(context));

            var stringComparison =
                context.Configuration.DefaultStringComparisonCulture.ToStringComparison(ignoreCase: true);
            
            return StringEqualityComparer.Get(stringComparison);
        }
    }
}
