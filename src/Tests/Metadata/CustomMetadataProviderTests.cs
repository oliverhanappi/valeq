using System.IO;
using System.Linq;
using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestInfrastructure;

namespace Valeq.Metadata
{
    [TestFixture]
    public class CustomMetadataProviderTests
    {
        private readonly Member _member1 = new Member("Test1", new object(), typeof(object), _ => null, t => true);
        private readonly Member _member2 = new Member("Test2", new object(), typeof(object), _ => null, t => true);
        
        [Test]
        public void Ctor_NoMetadata_ThrowsException()
        {
            Assert.That(() => new CustomMetadataProvider(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetTypeMetadata_Empty_ReturnsEmpty()
        {
            var metadataProvider = new CustomMetadataProvider(Enumerable.Empty<CustomMetadata>());
            var metadata = metadataProvider.GetTypeMetadata(typeof(object));

            Assert.That(metadata.IsEmpty);
        }
        
        [Test]
        public void GetTypeMetadata_NoType_ThrowsException()
        {
            var metadataProvider = new CustomMetadataProvider(Enumerable.Empty<CustomMetadata>());
            Assert.That(() => metadataProvider.GetTypeMetadata(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetTypeMetadata_ReturnsConfiguredMetadata()
        {
            var metadatas = new[]
            {
                new CustomTypeMetadata(typeof(Stream), new DefaultEqualityAttribute(), inherit: true),
                new CustomTypeMetadata(typeof(MemoryStream), new IgnoreInComparisonAttribute(), inherit: false),
                new CustomTypeMetadata(typeof(FileStream), new ValueEqualityAttribute(), inherit: false), 
            }.ProtectAgainstMultipleEnumeration();

            var metadata = new CustomMetadataProvider(metadatas).GetTypeMetadata(typeof(MemoryStream));

            Assert.That(metadata, Has.Count.EqualTo(2));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>());
        }
        
        [Test]
        public void GetMemberMetadata_NoMember_ThrowsException()
        {
            var metadataProvider = new CustomMetadataProvider(Enumerable.Empty<CustomMetadata>());
            Assert.That(() => metadataProvider.GetMemberMetadata(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetMemberMetadata_Empty_ReturnsEmpty()
        {
            var metadataProvider = new CustomMetadataProvider(Enumerable.Empty<CustomMetadata>());
            var metadata = metadataProvider.GetTypeMetadata(typeof(object));

            Assert.That(metadata.IsEmpty);
        }
        
        [Test]
        public void GetMemberMetadata_ReturnsConfiguredMetadata()
        {
            var metadatas = new[]
            {
                new CustomMemberMetadata(_member1, new DefaultEqualityAttribute()),
                new CustomMemberMetadata(_member1, new IgnoreInComparisonAttribute()),
                new CustomMemberMetadata(_member2, new ValueEqualityAttribute()), 
            }.ProtectAgainstMultipleEnumeration();

            var metadata = new CustomMetadataProvider(metadatas).GetMemberMetadata(_member1);

            Assert.That(metadata, Has.Count.EqualTo(2));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>());
        }
    }
}
