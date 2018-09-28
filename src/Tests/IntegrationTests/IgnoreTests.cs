using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Metadata;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class IgnoreTests
    {
        [Test]
        public void EqualityIsOnlyDeterminedByNonIgnoredProperties()
        {
            var x = new IgnorePoco {StringValue = "test1", IntegerValue = 42};
            var y = new IgnorePoco {StringValue = "test1", IntegerValue = 27};
            var z = new IgnorePoco {StringValue = "test2", IntegerValue = 27};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }

        private class IgnorePoco : ValueEquatable
        {
            public string StringValue { get; set; }

            [IgnoreInComparison]
            public int IntegerValue { get; set; }
        }
    }
}
