using System.Collections.Generic;
using NUnit.Framework;

namespace Valeq.Comparers
{
    [TestFixture]
    public class NullSafeEqualityComparerWrapperTests
    {
        private NullSafeEqualityComparerWrapper<TestPoco> _wrapper;

        [SetUp]
        public void SetUp()
        {
            _wrapper = new NullSafeEqualityComparerWrapper<TestPoco>(new TestPocoEqualityComparer());
        }

        [TestCase(null, null, ExpectedResult = true)]
        [TestCase(null, 1, ExpectedResult = false)]
        [TestCase(1, null, ExpectedResult = false)]
        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(1, 2, ExpectedResult = false)]
        public bool Equals(int? x, int? y)
        {
            var pocoX = x != null ? new TestPoco {Value = x.Value} : null;
            var pocoY = y != null ? new TestPoco {Value = y.Value} : null;

            return _wrapper.Equals(pocoX, pocoY);
        }

        [Test]
        public void GetHashCode_Null_ReturnsZero()
        {
            Assert.That(_wrapper.GetHashCode(null), Is.Zero);
        }

        [Test]
        public void GetHashCode_Instance_DelegatesToInner()
        {
            var hashCode = _wrapper.GetHashCode(new TestPoco {Value = 42});
            Assert.That(hashCode, Is.EqualTo(42));
        }

        private class TestPoco
        {
            public int Value { get; set; }
        }

        private class TestPocoEqualityComparer : IEqualityComparer<TestPoco>
        {
            public bool Equals(TestPoco x, TestPoco y) => x.Value == y.Value;
            public int GetHashCode(TestPoco obj) => obj.Value;
        }
    }
}
