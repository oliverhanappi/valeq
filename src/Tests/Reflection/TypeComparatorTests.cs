using System;
using System.Linq;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class TypeComparatorTests
    {
        [TestCase(typeof(A), typeof(B), ExpectedResult = -1)]
        [TestCase(typeof(B), typeof(A), ExpectedResult = 1)]
        [TestCase(typeof(A), typeof(A), ExpectedResult = 0)]
        [TestCase(typeof(A), typeof(C), ExpectedResult = -1)]
        [TestCase(typeof(C), typeof(A), ExpectedResult = 1)]
        [TestCase(typeof(C), typeof(D), ExpectedResult = 0)]
        [TestCase(null, typeof(A), ExpectedResult = 0)]
        [TestCase(typeof(A), null, ExpectedResult = 0)]
        [TestCase(null, null, ExpectedResult = 0)]
        public int Compare(Type x, Type y)
        {
            return new TypeComparator().Compare(x, y);
        }
        
        [Test]
        public void OrdersTypesByInheritance()
        {
            var unordered = new[] {typeof(C), typeof(A), typeof(B)};
            var ordered = unordered.OrderBy(x => x, new TypeComparator()).ToList();

            Assert.That(ordered, Is.EqualTo(new[] {typeof(A), typeof(B), typeof(C)}));
        }
        
        private class A {}
        private class B : A {}
        private class C : B {}
        private class D : B {}
    }
}
