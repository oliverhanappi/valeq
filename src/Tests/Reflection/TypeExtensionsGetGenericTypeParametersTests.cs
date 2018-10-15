using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class TypeExtensionsGetGenericTypeParametersTests
    {
        [Test]
        public void GetGenericTypeParameters_Direct()
        {
            var genericTypeParameters = typeof(TestClass<int>).GetGenericTypeParameters(typeof(TestClass<>));
            Assert.That(genericTypeParameters, Is.EqualTo(new[] {typeof(int)}));
        }
        
        [Test]
        public void GetGenericTypeParameters_Interface()
        {
            var genericTypeParameters = typeof(SubClass).GetGenericTypeParameters(typeof(ITestInterface<,>));
            Assert.That(genericTypeParameters, Is.EqualTo(new[] {typeof(int), typeof(bool)}));
        }

        [Test]
        public void GetGenericTypeParameters_NoType_ThrowsException()
        {
            Assert.That(() => TypeExtensions.GetGenericTypeParameters(null, typeof(TestClass<>)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void GetGenericTypeParameters_NullGenericTypeDefinition_ThrowsException()
        {
            Assert.That(() => typeof(SubClass).GetGenericTypeParameters(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetGenericTypeParameters_NoGenericTypeDefinition_ThrowsException()
        {
            Assert.That(() => typeof(SubClass).GetGenericTypeParameters(typeof(object)), Throws.ArgumentException);
        }

        [Test]
        public void GetGenericTypeParameters_MultipleImplementations_ThrowsException()
        {
            Assert.That(() => typeof(SubClass).GetGenericTypeParameters(typeof(ITestInterface2<>)),
                Throws.ArgumentException);
        }
        
        private class TestClass<T> : ITestInterface<T, bool>, ITestInterface2<int>, ITestInterface2<string>
        {
        }

        private class SubClass : TestClass<int>
        {
        }
        
        private interface ITestInterface<T1, T2>
        {
        }
        
        private interface ITestInterface2<T>
        {
        }
    }
}
