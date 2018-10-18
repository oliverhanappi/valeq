using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Valeq.Reflection;
using Valeq.Runtime;
using Valeq.Utils;

namespace Valeq.Metadata
{
    [TestFixture]
    public class MetadataCollectionTests
    {
        [Test]
        public void Count_Empty_ReturnsZero()
        {
            Assert.That(MetadataCollection.Empty.Count, Is.Zero);
        }

        [Test]
        public void Count_NonEmpty_ReturnsNumberOfImplementedMetadataInterfaces()
        {
            var metadata = MetadataCollection.ForMetadata(new TestMetadata());
            Assert.That(metadata, Has.Count.EqualTo(2));
        }

        [Test]
        public void IsEmpty_Empty_ReturnsTrue()
        {
            Assert.That(MetadataCollection.Empty.IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_NonEmpty_ReturnsFalse()
        {
            var metadata = MetadataCollection.ForMetadata(new TestMetadata());
            Assert.That(metadata.IsEmpty, Is.False);
        }

        [Test]
        public void HasMetadata_ExistingMetadata_ReturnsTrue()
        {
            var metadata = MetadataCollection.ForMetadata(new TestMetadata());
            Assert.That(metadata.HasMetadata<ICollectionCategoryMetadata>(), Is.True);
        }

        [Test]
        public void HasMetadata_NonExistingMetadata_ReturnsFalse()
        {
            var metadata = MetadataCollection.ForMetadata(new TestMetadata());
            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>(), Is.False);
        }

        [Test]
        public void HasMetadata_NonMetadataType_ThrowsException()
        {
            Assert.That(() => MetadataCollection.Empty.HasMetadata<TestMetadata>(), Throws.ArgumentException);
        }

        [Test]
        public void TryGetMetadata_ExistingMetadata_ReturnsSome()
        {
            var testMetadata = new TestMetadata();
            var metadata = MetadataCollection.ForMetadata(testMetadata);
            var result = metadata.TryGetMetadata<ICollectionCategoryMetadata>();

            Assert.That(result, Is.EqualTo(new OptionalValue<ICollectionCategoryMetadata>(testMetadata)));
        }

        [Test]
        public void TryGetMetadata_NonExistingMetadata_ReturnsFalse()
        {
            var metadata = MetadataCollection.ForMetadata(new TestMetadata());
            var result = metadata.TryGetMetadata<IIgnoredMemberMetadata>();

            Assert.That(result.HasValue, Is.False);
        }

        [Test]
        public void TryGetMetadata_NonMetadataType_ThrowsException()
        {
            Assert.That(() => MetadataCollection.Empty.TryGetMetadata<TestMetadata>(), Throws.ArgumentException);
        }

        [Test]
        public void OverrideWith_ReplacesOverlappingMetadataTypes()
        {
            var baseMetadata = new TestMetadata();
            var overrideMetadata = new BagAttribute();

            var metadata = MetadataCollection.ForMetadata(baseMetadata)
                .OverrideWith(MetadataCollection.ForMetadata(overrideMetadata));

            var result = metadata.TryGetMetadata<ICollectionCategoryMetadata>();
            Assert.That(result, Is.EqualTo(new OptionalValue<ICollectionCategoryMetadata>(overrideMetadata)));
        }

        [Test]
        public void OverrideWith_ContainsNonOverlappingBaseMetadataTypes()
        {
            var baseMetadata = new TestMetadata();
            var overrideMetadata = new BagAttribute();

            var metadata = MetadataCollection.ForMetadata(baseMetadata)
                .OverrideWith(MetadataCollection.ForMetadata(overrideMetadata));

            var result = metadata.TryGetMetadata<IElementEqualityComparerMetadata>();
            Assert.That(result, Is.EqualTo(new OptionalValue<IElementEqualityComparerMetadata>(baseMetadata)));
        }

        [Test]
        public void OverrideWith_ContainsNonOverlappingOverrideMetadataTypes()
        {
            var baseMetadata = new BagAttribute();
            var overrideMetadata = new TestMetadata();

            var metadata = MetadataCollection.ForMetadata(baseMetadata)
                .OverrideWith(MetadataCollection.ForMetadata(overrideMetadata));

            var result = metadata.TryGetMetadata<IElementEqualityComparerMetadata>();
            Assert.That(result, Is.EqualTo(new OptionalValue<IElementEqualityComparerMetadata>(overrideMetadata)));
        }

        [Test]
        public void OverrideWith_Null_ThrowsException()
        {
            Assert.That(() => MetadataCollection.Empty.OverrideWith(null), Throws.ArgumentNullException);
        }

        [Test]
        public void ForMetadata_BuildsMetadataCollection()
        {
            var metadata = MetadataCollection.ForMetadata(new TestMetadata());

            Assert.That(metadata.HasMetadata<ICollectionCategoryMetadata>(), Is.True);
            Assert.That(metadata.HasMetadata<IElementEqualityComparerMetadata>(), Is.True);
            Assert.That(metadata.HasMetadata<IEqualityComparerMetadata>(), Is.False);
        }

        [Test]
        public void ForMetadata_OverlappingMetadatas_ThrowsException()
        {
            Assert.That(() => MetadataCollection.ForMetadata(new TestMetadata(), new BagAttribute()),
                Throws.ArgumentException.With.Message.Contains("metadata types are provided multiple times"));
        }

        [Test]
        public void ForMetadata_Null_ThrowsException()
        {
            Assert.That(() => MetadataCollection.ForMetadata((IEnumerable<IMetadata>) null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Merge_Empty_ReturnsEmpty()
        {
            var metadata = MetadataCollection.Merge();
            Assert.That(metadata.IsEmpty, Is.True);
        }

        [Test]
        public void Merge_NonOverlapping_MergesAllMetadata()
        {
            var x = MetadataCollection.ForMetadata(new TestMetadata());
            var y = MetadataCollection.ForMetadata(new DefaultEqualityAttribute());
            var z = MetadataCollection.ForMetadata(new IgnoreCaseInComparisonAttribute());

            var merged = MetadataCollection.Merge(x, y, z);

            Assert.That(merged, Has.Count.EqualTo(4));
            Assert.That(merged.HasMetadata<ICollectionCategoryMetadata>(), Is.True);
            Assert.That(merged.HasMetadata<IEqualityComparisonTypeMetadata>(), Is.True);
            Assert.That(merged.HasMetadata<IEqualityComparerMetadata>(), Is.True);
            Assert.That(merged.HasMetadata<IElementEqualityComparerMetadata>(), Is.True);
        }

        [Test]
        public void Merge_Overlapping_ThrowsException()
        {
            var x = MetadataCollection.ForMetadata(new TestMetadata());
            var y = MetadataCollection.ForMetadata(new BagAttribute());

            Assert.That(() => MetadataCollection.Merge(x, y), Throws.ArgumentException);
        }

        [Test]
        public void Merge_Null_ThrowsException()
        {
            Assert.That(() => MetadataCollection.ForMetadata((IEnumerable<IMetadata>) null),
                Throws.ArgumentNullException);
        }

        private class TestMetadata : ICollectionCategoryMetadata, IElementEqualityComparerMetadata
        {
            public CollectionCategory GetCollectionCategory(EqualityComparerContext context)
            {
                throw new System.NotImplementedException();
            }

            public IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
