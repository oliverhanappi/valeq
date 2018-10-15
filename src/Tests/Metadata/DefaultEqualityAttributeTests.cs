using NUnit.Framework;
using Valeq.Configuration;

namespace Valeq.Metadata
{
    [TestFixture]
    public class DefaultEqualityAttributeTests
    {
        [Test]
        public void EqualityComparisonType_ReturnsDefaultEquality()
        {
            var equalityComparisonType = new DefaultEqualityAttribute().EqualityComparisonType;
            Assert.That(equalityComparisonType, Is.EqualTo(EqualityComparisonType.DefaultEquality));
        }
    }
}
