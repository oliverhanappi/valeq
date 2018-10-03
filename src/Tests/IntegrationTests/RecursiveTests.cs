using NUnit.Framework;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class RecursiveTests
    {
        [Test]
        public void IsValueEquatable()
        {
            var x = new RecursivePoco {Value = 42, Child = new RecursivePoco {Value = 27}};
            var y = new RecursivePoco {Value = 42, Child = new RecursivePoco {Value = 27}};
            var z = new RecursivePoco {Value = 42, Child = new RecursivePoco {Value = 28}};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }

        private class RecursivePoco : ValueEquatable
        {
            public int Value { get; set; }
            public RecursivePoco Child { get; set; }
            public RecursiveNullablePoco? NullableChild { get; set; }
        }

        private struct RecursiveNullablePoco
        {
            public RecursivePoco RecursivePoco { get; set; }
        }
    }
}
