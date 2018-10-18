using NUnit.Framework;
using Valeq.Configuration;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class DefaultEqualityAttributeTests
    {
        [Test]
        public void GetEqualityComparisonType_ReturnsDefaultEquality()
        {
            var equalityComparisonType =
                new DefaultEqualityAttribute().GetEqualityComparisonType(Create.An.EqualityComparerContext());
            Assert.That(equalityComparisonType, Is.EqualTo(EqualityComparisonType.DefaultEquality));
        }
    }
}
