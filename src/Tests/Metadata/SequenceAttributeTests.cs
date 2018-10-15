using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestDataBuilders;

namespace Valeq.Metadata
{
    [TestFixture]
    public class SequenceAttributeTests
    {
        [Test]
        public void GetCollectionCategory_ReturnsSequence()
        {
            var context = Create.An.EqualityComparerContext().ForType<int[]>();

            var collectionCategory = new SequenceAttribute().GetCollectionCategory(context);
            Assert.That(collectionCategory, Is.EqualTo(CollectionCategory.Sequence));
        }

        [Test]
        public void GetCollectionCategory_NonCollectionType_ThrowsException()
        {
            var context = Create.An.EqualityComparerContext().ForType<int>();
            Assert.That(() => new SequenceAttribute().GetCollectionCategory(context), Throws.ArgumentException
                .With.Message.Contains("Target type System.Int32 is not a collection."));
        }

        [Test]
        public void GetCollectionCategory_Null_ThrowsException()
        {
            Assert.That(() => new SequenceAttribute().GetCollectionCategory(null), Throws.ArgumentNullException);
        }
    }
}
