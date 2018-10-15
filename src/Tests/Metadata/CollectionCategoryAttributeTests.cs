using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class CollectionCategoryAttributeTests
    {
        [TestCase(CollectionCategory.Sequence)]
        [TestCase(CollectionCategory.Bag)]
        [TestCase(CollectionCategory.Set)]
        public void GetCollectionCategory_ReturnsCollectionCategory(CollectionCategory collectionCategory)
        {
            var context = Create.An.EqualityComparerContext().ForType<int[]>();

            var result = new CollectionCategoryAttribute(collectionCategory).GetCollectionCategory(context);
            Assert.That(result, Is.EqualTo(collectionCategory));
        }

        [Test]
        public void GetCollectionCategory_NonCollectionType_ThrowsException()
        {
            var context = Create.An.EqualityComparerContext().ForType<int>();
            Assert.That(
                () => new CollectionCategoryAttribute(CollectionCategory.Sequence).GetCollectionCategory(context),
                Throws.ArgumentException.With.Message.Contains("Target type System.Int32 is not a collection."));
        }

        [Test]
        public void GetCollectionCategory_Null_ThrowsException()
        {
            var attribute = new CollectionCategoryAttribute(CollectionCategory.Sequence);
            Assert.That(() => attribute.GetCollectionCategory(null), Throws.ArgumentNullException);
        }
    }
}
