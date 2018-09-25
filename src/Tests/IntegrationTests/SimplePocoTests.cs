using NUnit.Framework;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class SimplePocoTests
    {
        [Test]
        public void Equals_EqualInstances_ReturnsTrue()
        {
            var x = new SimplePoco {IntegerValue = 42};
            var y = new SimplePoco {IntegerValue = 42};

            Assert.That(x, Is.EqualTo(y));
        }

        [Test]
        public void EqualityOperator_EqualInstances_ReturnsTrue()
        {
            var x = new SimplePoco {IntegerValue = 42};
            var y = new SimplePoco {IntegerValue = 42};

            Assert.That(x == y, Is.True);
        }

        [Test]
        public void GetHashCode_EqualInstances_ReturnsSameHashCode()
        {
            var x = new SimplePoco {IntegerValue = 42};
            var y = new SimplePoco {IntegerValue = 42};

            Assert.That(y.GetHashCode(), Is.EqualTo(x.GetHashCode()));
        }

        [Test]
        public void Equals_NonEqualInstances_ReturnsFalse()
        {
            var x = new SimplePoco {IntegerValue = 42};
            var y = new SimplePoco {IntegerValue = 42, StringValue = "different"};

            Assert.That(x, Is.Not.EqualTo(y));
        }

        [Test]
        public void EqualityOperator_NonEqualInstances_ReturnsFalse()
        {
            var x = new SimplePoco {IntegerValue = 42};
            var y = new SimplePoco {IntegerValue = 42, StringValue = "different"};

            Assert.That(x == y, Is.False);
        }

        private class SimplePoco : ValueEquatable
        {
            public string StringValue { get; set; }
            public int IntegerValue { get; set; }
        }
    }
}