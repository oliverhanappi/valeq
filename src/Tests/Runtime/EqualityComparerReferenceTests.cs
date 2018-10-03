using NUnit.Framework;
using Valeq.TestInfrastructure;

namespace Valeq.Runtime
{
    [TestFixture]
    public class EqualityComparerReferenceTests
    {
        private AbsoluteIntegerEqualityComparer _inner;
        private EqualityComparerReference _equalityComparerReference;

        [SetUp]
        public void SetUp()
        {
            _inner = null;
            _equalityComparerReference = new EqualityComparerReference(() =>
            {
                _inner = new AbsoluteIntegerEqualityComparer();
                return _inner;
            });
        }

        [Test]
        public void Ctor_Null_ThrowsException()
        {
            Assert.That(() => new EqualityComparerReference(null), Throws.ArgumentNullException);
        }

        [Test]
        public void EvaluatesLazy()
        {
            Assert.That(_inner, Is.Null);
            _equalityComparerReference.Equals(1, 1);
            Assert.That(_inner, Is.Not.Null);
        }

        [Test]
        public void EvaluatesOnce()
        {
            _equalityComparerReference.Equals(1, 1);
            var inner1 = _inner;

            _equalityComparerReference.Equals(2, 2);
            var inner2 = _inner;

            Assert.That(inner2, Is.SameAs(inner1));
        }

        [Test]
        public void Equals_DelegatesToInner()
        {
            Assert.That(1, Is.EqualTo(-1).Using(_equalityComparerReference));
        }

        [Test]
        public void GetHashCode_DelegatesToInner()
        {
            var hashCodeReference = _equalityComparerReference.GetHashCode(-1);
            var hashCodeInner = _inner.GetHashCode(-1);

            Assert.That(hashCodeReference, Is.EqualTo(hashCodeInner));
        }
    }
}
