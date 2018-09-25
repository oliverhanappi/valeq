using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class CollectionCategoryExtensionsTests
    {
        [TestCase(typeof(List<int>), ExpectedResult = CollectionCategory.Sequence)]
        [TestCase(typeof(ArrayList), ExpectedResult = CollectionCategory.Sequence)]
        [TestCase(typeof(int[]), ExpectedResult = CollectionCategory.Sequence)]
        [TestCase(typeof(ICollection<int>), ExpectedResult = CollectionCategory.Bag)]
        [TestCase(typeof(ICollection), ExpectedResult = CollectionCategory.Bag)]
        [TestCase(typeof(BlockingCollection<int>), ExpectedResult = CollectionCategory.Bag)]
        [TestCase(typeof(HashSet<int>), ExpectedResult = CollectionCategory.Set)]
        [TestCase(typeof(IEnumerable<int>), ExpectedResult = CollectionCategory.Sequence)]
        public CollectionCategory GetCollectionCategory(Type type)
        {
            return type.GetCollectionCategory();
        }
    }
}
