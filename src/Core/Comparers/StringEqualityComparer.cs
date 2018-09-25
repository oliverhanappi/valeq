using System;

namespace Valeq.Comparers
{
    public static class StringEqualityComparer
    {
        public static StringComparer Get(StringComparison stringComparison)
        {
            switch (stringComparison)
            {
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;

                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;

                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;

                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;

                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;

                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;

                default:
                    throw new ArgumentException($"Unknown string comparison: {stringComparison}");
            }
        }
    }
}
