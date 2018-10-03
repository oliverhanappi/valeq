using System;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class MemberAccessTests
    {
        [Test]
        public void CreateFieldGetter_ReturnsDelegate()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.Field));

            var fieldGetter = MemberAccess.CreateFieldGetter(fieldInfo);

            var testClass = new TestClass {Field = 42};
            var value = fieldGetter.Invoke(testClass);

            Assert.That(value, Is.EqualTo(testClass.Field));
        }

        [Test]
        public void CreateFieldGetter_NoField_ThrowsArgumentNullException()
        {
            Assert.That(() => MemberAccess.CreateFieldGetter(null), Throws.ArgumentNullException);
        }

        [Test]
        public void CreateFieldGetter_NullableValue_DelegateReturnsNullable()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NullableField));
            var fieldGetter = MemberAccess.CreateFieldGetter(fieldInfo);

            var testClass = new TestClass {NullableField = 42};
            var value = fieldGetter.Invoke(testClass);

            Assert.That(value, Is.InstanceOf<int>());
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void CreateFieldGetter_NullableNull_DelegateReturnsNullable()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.NullableField));
            var fieldGetter = MemberAccess.CreateFieldGetter(fieldInfo);

            var testClass = new TestClass {NullableField = null};
            var value = fieldGetter.Invoke(testClass);

            Assert.That(value, Is.Null);
        }

        [Test]
        public void CreatePropertyGetter_ReturnsDelegate()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Property));

            var propertyGetter = MemberAccess.CreatePropertyGetter(propertyInfo);

            var testClass = new TestClass {Property = 42};
            var value = propertyGetter.Invoke(testClass);

            Assert.That(value, Is.EqualTo(testClass.Property));
        }

        [Test]
        public void CreatePropertyGetter_NoProperty_ThrowsArgumentNullException()
        {
            Assert.That(() => MemberAccess.CreatePropertyGetter(null), Throws.ArgumentNullException);
        }

        [Test]
        public void CreatePropertyGetter_NullableValue_DelegateReturnsNullable()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NullableProperty));
            var propertyGetter = MemberAccess.CreatePropertyGetter(propertyInfo);

            var testClass = new TestClass {NullableProperty = 42};
            var value = propertyGetter.Invoke(testClass);

            Assert.That(value, Is.InstanceOf<int>());
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void CreatePropertyGetter_NullableNull_DelegateReturnsNullable()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.NullableProperty));
            var propertyGetter = MemberAccess.CreatePropertyGetter(propertyInfo);

            var testClass = new TestClass {NullableProperty = null};
            var value = propertyGetter.Invoke(testClass);

            Assert.That(value, Is.Null);
        }

        private class TestClass
        {
            public int Field;
            public int Property { get; set; }

            public int? NullableField;
            public int? NullableProperty { get; set; }
        }
    }
}
