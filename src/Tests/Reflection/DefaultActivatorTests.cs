using NUnit.Framework;
using Valeq.TestDataBuilders;

namespace Valeq.Reflection
{
    [TestFixture]
    public class DefaultActivatorTests
    {
        private readonly IActivator _activator = new DefaultActivator();

        [Test]
        public void CreateInstance_NoType_ThrowsException()
        {
            var equalityComparerContext = Create.An.EqualityComparerContext();
            Assert.That(() => _activator.CreateInstance(null, equalityComparerContext), Throws.ArgumentNullException);
        }

        [Test]
        public void CreateInstance_NoContext_ThrowsException()
        {
            Assert.That(() => _activator.CreateInstance(typeof(object), null), Throws.ArgumentNullException);
        }

        [Test]
        public void CreateInstance_TypeWithoutParameterlessConstructor_ThrowsException()
        {
            var equalityComparerContext = Create.An.EqualityComparerContext();
            Assert.That(() => _activator.CreateInstance(typeof(TestClass), equalityComparerContext),
                Throws.ArgumentException);
        }

        [Test]
        public void CreateInstance_TypeWithParameterlessConstructor_CreatesInstance()
        {
            var equalityComparerContext = Create.An.EqualityComparerContext();
            var instance = _activator.CreateInstance(typeof(object), equalityComparerContext);

            Assert.That(instance, Is.InstanceOf<object>());
        }

        [Test]
        public void CreateInstance_Struct_ReturnsDefault()
        {
            var equalityComparerContext = Create.An.EqualityComparerContext();
            var instance = _activator.CreateInstance(typeof(TestStruct), equalityComparerContext);

            Assert.That(instance, Is.EqualTo(default(TestStruct)));
        }

        [Test]
        public void CreateInstance_Interface_ThrowsException()
        {
            var equalityComparerContext = Create.An.EqualityComparerContext();

            Assert.That(() => _activator.CreateInstance(typeof(ITestInterface), equalityComparerContext),
                Throws.Exception);
        }

        private class TestClass
        {
            public TestClass(int param)
            {
            }
        }

        private struct TestStruct
        {
        }

        private interface ITestInterface
        {
        }
    }
}
