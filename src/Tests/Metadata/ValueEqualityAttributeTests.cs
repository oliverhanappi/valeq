using NUnit.Framework;
using Valeq.Configuration;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class ValueEqualityAttributeTests
    {
        [Test]
        public void GetEqualityComparisonType_ReturnsValueEquality()
        {
            var equalityComparisonType =
                new ValueEqualityAttribute().GetEqualityComparisonType(Create.An.EqualityComparerContext());
            Assert.That(equalityComparisonType, Is.EqualTo(EqualityComparisonType.ValueEquality));
        }
    }
}