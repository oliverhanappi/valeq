using NUnit.Framework;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class IgnoreCaseInComparisonAttributeTests
    {
        [Test]
        public void GetEqualityComparer_ReturnsCaseInsensitiveStringComparer()
        {
            var context = Create.An.EqualityComparerContext().ForType<string>().Build();
            var equalityComparer = new IgnoreCaseInComparisonAttribute().GetEqualityComparer(context);

            Assert.That(equalityComparer, Is.Not.Null);
            Assert.That("HELLO", Is.EqualTo("hello").Using(equalityComparer));
        }
    }
}
