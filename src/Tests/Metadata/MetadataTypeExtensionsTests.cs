using System;
using System.Collections;
using NUnit.Framework;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [TestFixture]
    public class MetadataTypeExtensionsTests
    {
        [TestCase(typeof(object), ExpectedResult = false)]
        [TestCase(typeof(ICollectionCategoryMetadata), ExpectedResult = true)]
        [TestCase(typeof(BagAttribute), ExpectedResult = false)]
        public bool IsMetadataType(Type type)
        {
            return type.IsMetadataType();
        }

        [Test]
        public void IsMetadataType_Null_ThrowsException()
        {
            Assert.That(() => MetadataTypeExtensions.IsMetadataType(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetMetadataTypes_ReturnsMetadataTypes()
        {
            var metadataTypes = new BagAttribute().GetMetadataTypes();
            Assert.That(metadataTypes, Is.EquivalentTo(new[] {typeof(ICollectionCategoryMetadata)}));
        }

        [Test]
        public void GetMetadataTypes_Multiple_ReturnsAllMetadataTypes()
        {
            var metadataTypes = new TestMetadata().GetMetadataTypes();
            Assert.That(metadataTypes,
                Is.EquivalentTo(new[] {typeof(ICollectionCategoryMetadata), typeof(IElementEqualityComparerMetadata)}));
        }

        [Test]
        public void GetMetadataTypes_Null_ThrowsException()
        {
            Assert.That(() => MetadataTypeExtensions.GetMetadataTypes(null), Throws.ArgumentNullException);
        }

        private class TestMetadata : ICollectionCategoryMetadata, IElementEqualityComparerMetadata
        {
            public CollectionCategory GetCollectionCategory(EqualityComparerContext context)
            {
                throw new NotImplementedException();
            }

            public IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
