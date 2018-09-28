using System;
using System.Collections.Generic;

namespace Valeq.Comparers
{
    public static class StringEqualityComparer
    {
        public static IGenericEqualityComparer<string> Get(StringComparison stringComparison)
        {
            return new NullSafeEqualityComparerWrapper<string>(Get());
            
            StringComparer Get()
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
}
