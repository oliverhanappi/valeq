using System;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class TypeExtensionsIsAssignableToTests
    {
        [TestCase(typeof(TestClass), typeof(GenericClass<bool>), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(GenericClass<>), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(object), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(IInterface), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(IInterface<string>), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(IInterface<>), ExpectedResult = true)]
        [TestCase(typeof(TestClass), typeof(IInterface<int>), ExpectedResult = false)]
        [TestCase(typeof(IInterface<string>), typeof(IInterface<string>), ExpectedResult = true)]
        [TestCase(typeof(IInterface<string>), typeof(IInterface<>), ExpectedResult = true)]
        [TestCase(typeof(IInterface<>), typeof(IInterface<string>), ExpectedResult = false)]
        [TestCase(typeof(IInterface<string>), typeof(IInterface<int>), ExpectedResult = false)]
        [TestCase(typeof(IInterface<>), typeof(IInterface<>), ExpectedResult = true)]
        [TestCase(typeof(GenericClass<int>), typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(GenericClass<>), typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(BaseClass), typeof(GenericClass<>), ExpectedResult = false)]
        [TestCase(typeof(GenericClass<>), typeof(GenericClass<int>), ExpectedResult = false)]
        public bool IsAssignableTo(Type type, Type targetType)
        {
            return type.IsAssignableTo(targetType);
        }

        [Test]
        public void IsAssignableTo_NoSourceType_ThrowsException()
        {
            Assert.That(() => TypeExtensions.IsAssignableTo(null, GetType()), Throws.ArgumentNullException);
        }

        [Test]
        public void IsAssignableTo_NoTargetType_ThrowsException()
        {
            Assert.That(() => GetType().IsAssignableTo(null), Throws.ArgumentNullException);
        }
        
        private class TestClass : GenericClass<bool>, IInterface, IInterface<string>
        {
        }

        private class GenericClass<T> : BaseClass
        {
        }

        private class BaseClass
        {
        }

        private interface IInterface
        {
        }
        
        private interface IInterface<T>
        {
            
        }
    }
}
