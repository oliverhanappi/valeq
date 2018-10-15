using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    [TestFixture]
    public class AggregateMetadataProviderTests
    {
        private readonly Member _member = new Member("Test", new object(), typeof(object), _ => null, t =>  false);

        [Test]
        public void GetTypeMetadata_NoMetadataProviders_ReturnsEmptyMetadata()
        {
            var metadata = GetTypeMetadata();
            Assert.That(metadata.IsEmpty, Is.True);
        }
        
        [Test]
        public void GetTypeMetadata_SingleMetadataProvider_ReturnsMetadata()
        {
            var metadata = GetTypeMetadata(
                new[] {TypeMetadata(new DefaultEqualityAttribute())});

            Assert.That(metadata, Has.Count.EqualTo(1));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>(), Is.True);
        }

        [Test]
        public void GetTypeMetadata_DifferentMetadata_MergesMetadata()
        {
            var metadata = GetTypeMetadata(
                new[] {TypeMetadata(new DefaultEqualityAttribute())},
                new[] {TypeMetadata(new IgnoreInComparisonAttribute())});

            Assert.That(metadata, Has.Count.EqualTo(2));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>());
        }

        [Test]
        public void GetTypeMetadata_ConflictingMetadata_ThrowsException()
        {
            Assert.That(() => GetTypeMetadata(
                new[] {TypeMetadata(new DefaultEqualityAttribute())},
                new[] {TypeMetadata(new ValueEqualityAttribute())}), Throws.Exception);
        }

        [Test]
        public void GetMemberMetadata_NoMetadataProviders_ReturnsEmptyMetadata()
        {
            var metadata = GetMemberMetadata();
            Assert.That(metadata.IsEmpty, Is.True);
        }
        
        [Test]
        public void GetMemberMetadata_SingleMetadataProvider_ReturnsMetadata()
        {
            var metadata = GetMemberMetadata(
                new[] {MemberMetadata(new DefaultEqualityAttribute())});

            Assert.That(metadata, Has.Count.EqualTo(1));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>(), Is.True);
        }

        [Test]
        public void GetMemberMetadata_DifferentMetadata_MergesMetadata()
        {
            var metadata = GetMemberMetadata(
                new[] {MemberMetadata(new DefaultEqualityAttribute())},
                new[] {MemberMetadata(new IgnoreInComparisonAttribute())});

            Assert.That(metadata, Has.Count.EqualTo(2));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>());
        }

        [Test]
        public void GetMemberMetadata_ConflictingMetadata_ThrowsException()
        {
            Assert.That(() => GetMemberMetadata(
                new[] {MemberMetadata(new DefaultEqualityAttribute())},
                new[] {MemberMetadata(new ValueEqualityAttribute())}), Throws.Exception);
        }

        private CustomTypeMetadata TypeMetadata(IMetadata metadata)
        {
            return new CustomTypeMetadata(typeof(object), metadata, inherit: false);
        }
        
        private MetadataCollection GetTypeMetadata(params IEnumerable<CustomTypeMetadata>[] metadataGroups)
        {
            var aggregateMetadataProvider =
                new AggregateMetadataProvider(metadataGroups.Select(g => new CustomMetadataProvider(g)));
            return aggregateMetadataProvider.GetTypeMetadata(typeof(object));
        }

        private CustomMemberMetadata MemberMetadata(IMetadata metadata)
        {
            return new CustomMemberMetadata(_member, metadata);
        }

        private MetadataCollection GetMemberMetadata(params IEnumerable<CustomMemberMetadata>[] metadataGroups)
        {
            var aggregateMetadataProvider =
                new AggregateMetadataProvider(metadataGroups.Select(g => new CustomMetadataProvider(g)));
            return aggregateMetadataProvider.GetMemberMetadata(_member);
        }
    }
}
