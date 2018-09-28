using System;
using NUnit.Framework;

namespace Valeq.Comparers
{
    [TestFixture]
    public class StringEqualityComparerTests
    {
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
