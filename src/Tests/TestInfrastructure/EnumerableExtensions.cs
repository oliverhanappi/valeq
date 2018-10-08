using System;
using System.Collections.Generic;

namespace Valeq.TestInfrastructure
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ProtectAgainstMultipleEnumeration<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            var enumerated = false;
            return Enumerate();

            IEnumerable<T> Enumerate()
            {
                if (enumerated)
                    throw new InvalidOperationException($"Enumerable {enumerable} has already been enumerated.");

                enumerated = true;
                foreach (var item in enumerable)
                    yield return item;
            }
        }
    }
}
