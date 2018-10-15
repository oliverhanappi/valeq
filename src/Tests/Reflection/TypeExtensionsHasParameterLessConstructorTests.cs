using System;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class TypeExtensionsHasParameterLessConstructorTests
    {
        [TestCase(typeof(PublicParameterlessConstructorClass), ExpectedResult = true)]
        [TestCase(typeof(PrivateParameterlessConstructorClass), ExpectedResult = true)]
        [TestCase(typeof(NoParameterlessConstructorClass), ExpectedResult = false)]
        public bool HasParameterLessConstructor(Type type)
        {
            return type.HasParameterLessConstructor();
        }

        [Test]
        public void HasParameterLessConstructor_NoType_ThrowsException()
        {
            Assert.That(() => TypeExtensions.HasParameterLessConstructor(null), Throws.ArgumentNullException);
        }
        
        private class PublicParameterlessConstructorClass
        {
        }

        private class PrivateParameterlessConstructorClass
        {
            private PrivateParameterlessConstructorClass()
            {
            }
        }

        private class NoParameterlessConstructorClass
        {
            public NoParameterlessConstructorClass(int value)
            {
            }
        }
    }
}
