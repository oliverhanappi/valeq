using System;
using NUnit.Framework;
using Valeq.Configuration;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class StringComparisonTests
    {
        [Test]
        public void ComparesStringsUsingConfiguredMethod()
        {
            var x = new StringPoco {Value = "Hello World"};
            var y = new StringPoco {Value = "HELLO world"};
            var z = new StringPoco {Value = "Hallo Welt"};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }

        private class StringPoco : ValueEquatable
        {
            [StringComparison(StringComparison.InvariantCultureIgnoreCase)]
            public string Value { get; set; }
        }
    }
}
