using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Runtime;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class DefaultEqualityComparisonTypeTests
    {
        private TestPoco _testPoco1;
        private TestPoco _testPoco2;
        private TestPoco _testPoco3;

        [SetUp]
        public void SetUp()
        {
            var value1 = new object();
            var value2 = new object();

            _testPoco1 = new TestPoco {Value = value1};
            _testPoco2 = new TestPoco {Value = value1};
            _testPoco3 = new TestPoco {Value = value2};
        }
        
        [Test]
        public void DefaultEquality_AppliesValueEqualityOnlyToValueEqualityMarkedTypes()
        {
            ValueEqualityComparerProvider.Configure(b =>
                b.DefaultEqualityComparisonType = EqualityComparisonType.DefaultEquality);

            Assert.That(_testPoco1, Is.EqualTo(_testPoco2));
            Assert.That(_testPoco1, Is.Not.EqualTo(_testPoco3));
        }

        [Test]
        public void ValueEquality_AppliesValueEqualityToAllTypes()
        {
            ValueEqualityComparerProvider.Configure(b =>
                b.DefaultEqualityComparisonType = EqualityComparisonType.ValueEquality);

            Assert.That(_testPoco1, Is.EqualTo(_testPoco2));
            Assert.That(_testPoco1, Is.EqualTo(_testPoco3));
        }

        private class TestPoco : ValueEquatable
        {
            public object Value { get; set; }
        }
    }
}
