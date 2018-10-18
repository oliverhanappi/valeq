using FakeItEasy;
using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Reflection;
using Valeq.TestDataBuilders;
using Valeq.TestInfrastructure;

namespace Valeq.Metadata
{
    [TestFixture]
    public class ElementEqualityComparerAttributeTests
    {
        [Test]
        public void Ctor_Null_ThrowsException()
        {
            Assert.That(() => new ElementEqualityComparerAttribute(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void Ctor_NonEqualityComparerType_ThrowsException()
        {
            Assert.That(() =>  new ElementEqualityComparerAttribute(typeof(object)), Throws.ArgumentException);
        }

        [Test]
        public void GetElementEqualityComparer_NoContext_ThrowsException()
        {
            var attribute = new ElementEqualityComparerAttribute(typeof(AbsoluteIntegerEqualityComparer));
            Assert.That(() => attribute.GetElementEqualityComparer(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetElementEqualityComparer_ActivatesEqualityComparerType()
        {
            var activator = A.Fake<IActivator>(o => o.Strict());
            var configuration = new ValueEqualityConfigurationBuilder {Activator = activator}.Build();
            var context = Create.An.EqualityComparerContext().WithConfiguration(configuration).Build();

            var equalityComparer = new AbsoluteIntegerEqualityComparer();
            A.CallTo(() => activator.CreateInstance(typeof(AbsoluteIntegerEqualityComparer), context))
                .Returns(equalityComparer).Once();
            
            var attribute = new ElementEqualityComparerAttribute(typeof(AbsoluteIntegerEqualityComparer));
            var result = attribute.GetElementEqualityComparer(context);

            Assert.That(result, Is.SameAs(equalityComparer));
        }
    }
}
