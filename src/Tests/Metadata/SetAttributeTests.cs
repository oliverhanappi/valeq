using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class SetAttributeTests
    {
        [Test]
        public void GetCollectionCategory_ReturnsSet()
        {
            var context = Create.An.EqualityComparerContext().ForType<int[]>();

            var collectionCategory = new SetAttribute().GetCollectionCategory(context);
            Assert.That(collectionCategory, Is.EqualTo(CollectionCategory.Set));
        }

        [Test]
        public void GetCollectionCategory_NonCollectionType_ThrowsException()
        {
            var context = Create.An.EqualityComparerContext().ForType<int>();
            Assert.That(() => new SetAttribute().GetCollectionCategory(context), Throws.ArgumentException
                .With.Message.Contains("Target type System.Int32 is not a collection."));
        }

        [Test]
        public void GetCollectionCategory_Null_ThrowsException()
        {
            Assert.That(() => new SetAttribute().GetCollectionCategory(null), Throws.ArgumentNullException);
        }
    }
}
