using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Valeq.Comparers
{
    [TestFixture]
    public class DefaultEqualityComparerTests
    {
        [Test]
        public void GetForType_Null_ThrowsException()
        {
            Assert.That(() => DefaultEqualityComparer.GetForType(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetForType_ReturnsEqualityComparerOfThatType()
        {
            var equalityComparer1 = DefaultEqualityComparer.GetForType(typeof(DateTime));
            var equalityComparer2 = EqualityComparer<DateTime>.Default;

            Assert.That(equalityComparer1, Is.SameAs(equalityComparer2));
        }
        
        [Test]
        public void GetForType_DoesNotFailHashCodeCalculationOnNull()
        {
            var equalityComparer = DefaultEqualityComparer.GetForType(typeof(string));
            Assert.That(() => equalityComparer.GetHashCode(null), Throws.Nothing);
        }
    }
}
