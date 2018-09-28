using System;

namespace Valeq.Configuration
{
    public static class StringComparisonCultureExtensions
    {
        public static StringComparison ToStringComparison(this StringComparisonCulture stringComparisonCulture,
            bool ignoreCase)
        {
            switch (stringComparisonCulture)
            {
                case StringComparisonCulture.None:
                    return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                case StringComparisonCulture.Current:
                    return ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

                case StringComparisonCulture.Invariant:
                    return ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

                default:
                    var message = $"Unknown string comparison culture: {stringComparisonCulture}";
                    throw new ArgumentException(message, nameof(stringComparisonCulture));
            }
        }
    }
}
