using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class PropertyInfoExtensionsTests
    {
        [Test]
        public void GetRootPropertyInfo_Null_ThrowsException()
        {
            Assert.That(() => PropertyInfoExtensions.GetRootPropertyInfo(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetRootPropertyInfo_Root_ReturnsSelf()
        {
            var rootPropertyInfo = typeof(A).GetProperty(nameof(A.Property));

            var result = rootPropertyInfo.GetRootPropertyInfo();
            Assert.That(result, Is.SameAs(rootPropertyInfo));
        }
        
        [Test]
        public void GetRootPropertyInfo_Derived_ReturnsPropertyOnRootClass()
        {
            var derivedPropertyInfo = typeof(B).GetProperty(nameof(B.Property));
            var rootPropertyInfo = typeof(A).GetProperty(nameof(A.Property));

            var result = derivedPropertyInfo.GetRootPropertyInfo();
            Assert.That(result, Is.SameAs(rootPropertyInfo));
        }
        
        [Test]
        public void GetRootPropertyInfo_DerivedMultipleTimes_ReturnsPropertyOnRootClass()
        {
            var derivedPropertyInfo = typeof(C).GetProperty(nameof(C.Property));
            var rootPropertyInfo = typeof(A).GetProperty(nameof(A.Property));

            var result = derivedPropertyInfo.GetRootPropertyInfo();
            Assert.That(result, Is.SameAs(rootPropertyInfo));
        }
        
        private class A
        {
            public virtual int Property { get; set; }
        }

        private class B : A
        {
            public override int Property { get; set; }
        }

        private class C : B
        {
            public override int Property { get; set; }
        }
    }
}
