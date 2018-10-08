using System;
using NUnit.Framework;

namespace Valeq.Comparers
{
    [TestFixture]
    public class StringEqualityComparerTests
    {
        [TestCase(StringComparison.Ordinal, null, null, ExpectedResult = true)]
        [TestCase(StringComparison.Ordinal, null, "", ExpectedResult = false)]
        [TestCase(StringComparison.Ordinal, "", null, ExpectedResult = false)]
        [TestCase(StringComparison.Ordinal, "hello", "hello", ExpectedResult = true)]
        [TestCase(StringComparison.Ordinal, "hello", "HELLO", ExpectedResult = false)]
        [TestCase(StringComparison.OrdinalIgnoreCase, "hello", "HELLO", ExpectedResult = true)]
        [TestCase(StringComparison.OrdinalIgnoreCase, "hello", "world", ExpectedResult = false)]
        public bool Equals(StringComparison stringComparison, string x, string y)
        {
            var equalityComparer = StringEqualityComparer.Get(stringComparison);
            return equalityComparer.Equals(x, y);
        }

        [TestCase(StringComparison.Ordinal, null, null)]
        [TestCase(StringComparison.Ordinal, "hello", "hello")]
        [TestCase(StringComparison.OrdinalIgnoreCase, "hello", "HELLO")]
        public void GetHashCode_EqualStrings_ReturnsSameHashCode(StringComparison stringComparison, string x, string y)
        {
            var equalityComparer = StringEqualityComparer.Get(stringComparison);

            var hashCodeX = equalityComparer.GetHashCode(x);
            var hashCodeY = equalityComparer.GetHashCode(y);

            Assert.That(hashCodeX, Is.EqualTo(hashCodeY));
        }

        [TestCase(null, "value")]
        [TestCase("value", null)]
        [TestCase(null, null)]
        public void Equals_Null_DoesNotFail(string x, string y)
        {
            var stringComparer = StringEqualityComparer.Get(StringComparison.Ordinal);
            Assert.That(() => stringComparer.Equals(x, y), Throws.Nothing);
        }

        [Test]
        public void GetHashCode_Null_DoesNotFail()
        {
            var stringComparer = StringEqualityComparer.Get(StringComparison.Ordinal);
            Assert.That(() => stringComparer.GetHashCode(null), Throws.Nothing);
        }
    }
}
