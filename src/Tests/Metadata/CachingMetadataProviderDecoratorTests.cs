using FakeItEasy;
using NUnit.Framework;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    [TestFixture]
    public class CachingMetadataProviderDecoratorTests
    {
        private IMetadataProvider _inner;
        private CachingMetadataProviderDecorator _decorator;
        private Member _member;

        [SetUp]
        public void SetUp()
        {
            _inner = A.Fake<IMetadataProvider>(o => o.Strict());
            _decorator = new CachingMetadataProviderDecorator(_inner);
            _member = new Member("Test", new object(), typeof(object), _ => null, t => true);
        }

        [Test]
        public void GetTypeMetadata_ReturnsResultFromInner()
        {
            var metadata = MetadataCollection.ForMetadata(new[] {new ValueEqualityAttribute()});
            A.CallTo(() => _inner.GetTypeMetadata(typeof(object))).Returns(metadata);

            var result = _decorator.GetTypeMetadata(typeof(object));
            Assert.That(result, Is.SameAs(metadata));
        }

        [Test]
        public void GetTypeMetadata_CachesResult()
        {
            var metadata = MetadataCollection.ForMetadata(new[] {new ValueEqualityAttribute()});
            A.CallTo(() => _inner.GetTypeMetadata(typeof(object))).Returns(metadata).Once();

            var result1 = _decorator.GetTypeMetadata(typeof(object));
            var result2 = _decorator.GetTypeMetadata(typeof(object));
            
            Assert.That(result2, Is.SameAs(result1));
            A.CallTo(() => _inner.GetTypeMetadata(typeof(object))).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetMemberMetadata_ReturnsResultFromInner()
        {
            var metadata = MetadataCollection.ForMetadata(new[] {new ValueEqualityAttribute()});
            A.CallTo(() => _inner.GetMemberMetadata(_member)).Returns(metadata);

            var result = _decorator.GetMemberMetadata(_member);
            Assert.That(result, Is.SameAs(metadata));
        }

        [Test]
        public void GetMemberMetadata_CachesResult()
        {
            var metadata = MetadataCollection.ForMetadata(new[] {new ValueEqualityAttribute()});
            A.CallTo(() => _inner.GetMemberMetadata(_member)).Returns(metadata).Once();

            var result1 = _decorator.GetMemberMetadata(_member);
            var result2 = _decorator.GetMemberMetadata(_member);
            
            Assert.That(result2, Is.SameAs(result1));
            A.CallTo(() => _inner.GetMemberMetadata(_member)).MustHaveHappenedOnceExactly();
        }
    }
}
