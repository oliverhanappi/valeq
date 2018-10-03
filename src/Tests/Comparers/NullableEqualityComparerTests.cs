using System;
using System.Collections;
using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestInfrastructure;

namespace Valeq.Comparers
{
    [TestFixture]
    public class NullableEqualityComparerTests
    {
        private IEqualityComparer _nullableEqualityComparer;

        [SetUp]
        public void SetUp()
        {
            _nullableEqualityComparer =
                NullableEqualityComparer.Create(typeof(int), new AbsoluteIntegerEqualityComparer());
        }

        [TestCase(null, null, ExpectedResult = true)]
        [TestCase(1, null, ExpectedResult = false)]
        [TestCase(null, 1, ExpectedResult = false)]
        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(1, -1, ExpectedResult = true)]
        [TestCase(1, 2, ExpectedResult = false)]
        public bool Equals(int? x, int? y)
        {
            return _nullableEqualityComparer.Equals(x, y);
        }

        [TestCase(null, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(-1, ExpectedResult = 1)]
        public int GetHashCode(int? obj)
        {
            return _nullableEqualityComparer.GetHashCode(obj);
        }

        [TestCase(typeof(object))]
        [TestCase(typeof(IDisposable))]
        [TestCase(typeof(Member))]
        public void Create_NonValueType_ThrowsException(Type underlyingType)
        {
            var equalityComparer = DefaultEqualityComparer.GetForType(underlyingType);
            Assert.That(() => NullableEqualityComparer.Create(underlyingType, equalityComparer),
                Throws.ArgumentException);
        }

        [Test]
        public void Create_NonMatchingEqualityComparer_ThrowsException()
        {
            Assert.That(() => NullableEqualityComparer.Create(typeof(int), StringComparer.Ordinal),
                Throws.ArgumentException);
        }
    }
}
