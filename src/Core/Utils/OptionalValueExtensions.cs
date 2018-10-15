using System;

namespace Valeq.Utils
{
    public static class OptionalValueExtensions
    {
        public static T IfNone<T>(this OptionalValue<T> optionalValue, Func<T> fallbackFactory)
        {
            if (fallbackFactory == null) throw new ArgumentNullException(nameof(fallbackFactory));
            return optionalValue.Match(x => x, fallbackFactory);
        }

        public static T IfNone<T>(this OptionalValue<T> optionalValue, T fallbackValue)
        {
            return optionalValue.IfNone(() => fallbackValue);
        }
    }
}
