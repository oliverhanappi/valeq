using NUnit.Framework;
using Valeq.Configuration;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class PropertySearchScopeAttributeTests
    {
        [Test]
        public void GetPropertySearchScope_ReturnsPropertySearchScope()
        {
            var attribute = new PropertySearchScopeAttribute(PropertySearchScope.All);
            var propertySearchScope = attribute.GetPropertySearchScope(Create.An.EqualityComparerContext());

            Assert.That(propertySearchScope, Is.EqualTo(PropertySearchScope.All));
        }
    }
}
