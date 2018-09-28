using NUnit.Framework;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class NullableTests
    {
        [Test]
        public void ComparesNullableValueByValueEquality()
        {
            var nullablePoco1 = new NullablePoco {TestStruct = new TestStruct {Value = 42}};
            var nullablePoco2 = new NullablePoco {TestStruct = new TestStruct {Value = 42}};
            var nullablePoco3 = new NullablePoco {TestStruct = new TestStruct {Value = 27}};

            Assert.That(nullablePoco1, Is.EqualTo(nullablePoco2));
            Assert.That(nullablePoco1, Is.Not.EqualTo(nullablePoco3));
        }

        [Test]
        public void HandlesNull()
        {
            var nullablePoco1 = new NullablePoco {TestStruct = null};
            var nullablePoco2 = new NullablePoco {TestStruct = null};

            Assert.That(nullablePoco1, Is.EqualTo(nullablePoco2));
        }
        
        private class NullablePoco : ValueEquatable
        {
            public TestStruct? TestStruct { get; set; }
        }

        private struct TestStruct
        {
            public int Value { get; set; }
        }
    }
}
