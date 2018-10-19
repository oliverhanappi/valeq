using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class ExpressionUtilityTests
    {
        [Test]
        public void GetPropertyInfo_ReturnsPropertyInfo()
        {
            var propertyInfo = ExpressionUtility.GetPropertyInfo((TestClass c) => c.Target);
            var expectedPropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Target));

            Assert.That(propertyInfo, Is.SameAs(expectedPropertyInfo));
        }

        [Test]
        public void GetPropertyInfo_NoExpression_ThrowsException()
        {
            Assert.That(() => ExpressionUtility.GetPropertyInfo<TestClass, string>(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetPropertyInfo_Method_ThrowsException()
        {
            Assert.That(() => ExpressionUtility.GetPropertyInfo((TestClass c) => c.Method()), Throws.ArgumentException);
        }

        [Test]
        public void GetPropertyInfo_Field_ThrowsException()
        {
            Assert.That(() => ExpressionUtility.GetPropertyInfo((TestClass c) => c.Field), Throws.ArgumentException);
        }

#pragma warning disable 649        

        private class TestClass
        {
            public string Field;
            public string Target { get; set; }

            public string Method() => Target;
        }
    }
}
