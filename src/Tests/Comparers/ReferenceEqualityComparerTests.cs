using NUnit.Framework;

namespace Valeq.Comparers
{
    [TestFixture]
    public class ReferenceEqualityComparerTests
    {
        private readonly ReferenceEqualityComparer<TestPoco> _equalityComparer = new ReferenceEqualityComparer<TestPoco>();

        [TestCase(null, null, ExpectedResult = true)]
        [TestCase(null, 0, ExpectedResult = false)]
        [TestCase(0, null, ExpectedResult = false)]
        [TestCase(0, 0, ExpectedResult = true)]
        [TestCase(0, 1, ExpectedResult = false)]
        public bool Equals(int? x, int? y)
        {
            var pocos = new[] {new TestPoco {Value = 42}, new TestPoco {Value = 42}};
            var pocoX = x.HasValue ? pocos[x.Value] : null;
            var pocoY = y.HasValue ? pocos[y.Value] : null;
            
            return _equalityComparer.Equals(pocoX, pocoY);
        }

        [Test]
        public void GetHashCode_Null_ReturnsZero()
        {
            var hashCode = _equalityComparer.GetHashCode(null);
            Assert.That(hashCode, Is.Zero);
        }

        [Test]
        public void GetHashCode_SameInstance_ReturnsSameHashCode()
        {
            var testPoco = new TestPoco {Value = 42};
            var hashCode1 = _equalityComparer.GetHashCode(testPoco);

            testPoco.Value = 27;
            var hashCode2 = _equalityComparer.GetHashCode(testPoco);

            Assert.That(hashCode1, Is.EqualTo(hashCode2));
        }
        
        private class TestPoco
        {
            public int Value { get; set; }
        }
    }
}
