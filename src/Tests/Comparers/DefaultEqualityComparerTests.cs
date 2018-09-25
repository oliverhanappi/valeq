using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Valeq.Comparers
{
    [TestFixture]
    public class DefaultEqualityComparerTests
    {
        [Test]
        public void GetForType_ReturnsEqualityComparerOfThatType()
        {
            var equalityComparer1 = DefaultEqualityComparer.GetForType(typeof(DateTime));
            var equalityComparer2 = EqualityComparer<DateTime>.Default;

            Assert.That(equalityComparer1, Is.SameAs(equalityComparer2));
        }
    }
}