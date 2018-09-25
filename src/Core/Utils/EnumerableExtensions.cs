using System;
using System.Collections.Generic;
using System.Linq;

namespace Valeq.Utils
{
    public static class EnumerableExtensions
    {
        public static bool TryGetCount<T>(this IEnumerable<T> enumerable, out int count)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            if (enumerable is IReadOnlyCollection<T> readOnlyCollection)
            {
                count = readOnlyCollection.Count;
                return true;
            }

            if (enumerable is ICollection<T> collection)
            {
                count = collection.Count;
                return true;
            }

            count = -1;
            return false;
        }

        /// <remarks>
        /// Code inspired by https://stackoverflow.com/a/13503860/128709
        /// </remarks>
        public static IEnumerable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftEnumerable,
            IEnumerable<TRight> rightEnumerable,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TKey, TLeft, TRight, TResult> resultSelector,
            TLeft leftDefault = default(TLeft),
            TRight rightDefault = default(TRight),
            IEqualityComparer<TKey> keyComparer = null)
        {
            if (leftEnumerable == null) throw new ArgumentNullException(nameof(leftEnumerable));
            if (rightEnumerable == null) throw new ArgumentNullException(nameof(rightEnumerable));
            if (leftKeySelector == null) throw new ArgumentNullException(nameof(leftKeySelector));
            if (rightKeySelector == null) throw new ArgumentNullException(nameof(rightKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;

            var leftLookup = leftEnumerable.ToLookup(leftKeySelector, keyComparer);
            var rightLookup = rightEnumerable.ToLookup(rightKeySelector, keyComparer);

            var keys = new HashSet<TKey>(leftLookup.Select(p => p.Key), keyComparer);
            keys.UnionWith(rightLookup.Select(p => p.Key));

            var join =
                from key in keys
                from left in leftLookup[key].DefaultIfEmpty(leftDefault)
                from right in rightLookup[key].DefaultIfEmpty(rightDefault)
                select resultSelector(key, left, right);

            return join;
        }
    }
}
