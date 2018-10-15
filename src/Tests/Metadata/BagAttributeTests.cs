using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class BagAttributeTests
    {
        [Test]
        public void GetCollectionCategory_ReturnsBag()
        {
            var context = Create.An.EqualityComparerContext().ForType<int[]>();

            var collectionCategory = new BagAttribute().GetCollectionCategory(context);
            Assert.That(collectionCategory, Is.EqualTo(CollectionCategory.Bag));
        }

        [Test]
        public void GetCollectionCategory_NonCollectionType_ThrowsException()
        {
            var context = Create.An.EqualityComparerContext().ForType<int>();
            Assert.That(() => new BagAttribute().GetCollectionCategory(context), Throws.ArgumentException
                .With.Message.Contains("Target type System.Int32 is not a collection."));
        }

        [Test]
        public void GetCollectionCategory_Null_ThrowsException()
        {
            Assert.That(() => new BagAttribute().GetCollectionCategory(null), Throws.ArgumentNullException);
        }
    }
}
