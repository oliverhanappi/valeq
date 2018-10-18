using FakeItEasy;
using NUnit.Framework;
using Valeq.TestInfrastructure;

namespace Valeq.Runtime
{
    [TestFixture]
    public class ValueEqualityComparerProviderExtensionsTests
    {
        [Test]
        public void GetEqualityComparer_Type_ReturnsCastedEqualityComparer()
        {
            var equalityComparer = new AbsoluteIntegerEqualityComparer();
            
            var provider = A.Fake<IValueEqualityComparerProvider>(o => o.Strict());
            A.CallTo(() => provider.GetEqualityComparer(typeof(int))).Returns(equalityComparer).Once();

            var result = provider.GetEqualityComparer<int>();
            Assert.That(result, Is.SameAs(equalityComparer));
        }

        [Test]
        public void GetEqualityComparer_Null_ThrowsException()
        {
            Assert.That(() => ValueEqualityComparerProviderExtensions.GetEqualityComparer<int>(null),
                Throws.ArgumentNullException);
        }
    }
}
