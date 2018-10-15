using System;
using NUnit.Framework;

namespace Valeq.Utils
{
    [TestFixture]
    public class OptionalValueExtensionsTests
    {
        private OptionalValue<int> _some;
        private OptionalValue<int> _none;

        [SetUp]
        public void SetUp()
        {
            _some = 42;
            _none = OptionalValue<int>.None;
        }

        [Test]
        public void IfNone_NoFallbackFactory_ThrowsException()
        {
            Assert.That(() => _some.IfNone(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void IfNone_FallbackFactory_Some_ReturnsValue()
        {
            var value = _some.IfNone(() => throw new Exception());
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void IfNone_FallbackValue_Some_ReturnsValue()
        {
            var value = _some.IfNone(27);
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void IfNone_FallbackFactory_None_ReturnsResultOfFallbackFactory()
        {
            var value = _none.IfNone(() => 27);
            Assert.That(value, Is.EqualTo(27));
        }

        [Test]
        public void IfNone_FallbackValue_None_ReturnsFallbackValue()
        {
            var value = _none.IfNone(27);
            Assert.That(value, Is.EqualTo(27));
        }
    }
}
