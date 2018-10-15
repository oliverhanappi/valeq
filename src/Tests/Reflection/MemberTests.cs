using System;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class MemberTests
    {
        [Test]
        public void FromPropertyInfo_NoPropertyInfo_ThrowsException()
        {
            Assert.That(() => Member.FromPropertyInfo(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void FromFieldInfo_NoFieldInfo_ThrowsException()
        {
            Assert.That(() => Member.FromFieldInfo(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void Name_Property()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            Assert.That(member.Name, Is.EqualTo("Property Valeq.Reflection.MemberTests.TestClass.Property"));
        }
        
        [Test]
        public void ToString_Property()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            Assert.That(member.ToString(),
                Is.EqualTo("Property Valeq.Reflection.MemberTests.TestClass.Property: System.String"));
        }

        [Test]
        public void Name_Field()
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            Assert.That(member.Name, Is.EqualTo("Field Valeq.Reflection.MemberTests.TestClass.Field"));
        }

        [Test]
        public void ToString_Field()
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            Assert.That(member.ToString(),
                Is.EqualTo("Field Valeq.Reflection.MemberTests.TestClass.Field: System.Int32"));
        }

        [Test]
        public void GetValue_Property_ReturnsValue()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            var value = member.GetValue(new TestClass {Property = "Hello"});
            Assert.That(value, Is.EqualTo("Hello"));
        }

        [Test]
        public void GetValue_Property_WrongType_ThrowsException()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            Assert.That(() => member.GetValue(new object()), Throws.ArgumentException);
        }

        [Test]
        public void GetValue_Property_Null_ThrowsException()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            Assert.That(() => member.GetValue(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetValue_Property_GetterFails_WrapsException()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            var testClass = new TestClass {Throw = true};
            var exception = Assert.Throws<MemberRetrievalException>(() => member.GetValue(testClass));
            Assert.That(exception.InnerException, Has.Message.EqualTo("oops"));
        }

        [Test]
        public void GetValue_Field_ReturnsValue()
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            var value = member.GetValue(new TestClass {Field = 42});
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void GetValue_Field_WrongType_ThrowsException()
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            Assert.That(() => member.GetValue(new object()), Throws.ArgumentException);
        }

        [Test]
        public void GetValue_Field_Null_ThrowsException()
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            Assert.That(() => member.GetValue(null), Throws.ArgumentNullException);
        }

        [TestCase(typeof(TestClass), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Property(Type type)
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            return member.IsPartOf(type);
        }

        [TestCase(typeof(TestClass), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Field(Type type)
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            return member.IsPartOf(type);
        }

        [Test]
        public void MemberType_Property_ReturnsPropertyType()
        {
            var member = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            Assert.That(member.MemberType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void MemberType_Field_ReturnsFieldType()
        {
            var member = Member.FromFieldInfo(typeof(TestClass).GetField(nameof(TestClass.Field)));
            Assert.That(member.MemberType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void MemberSource_Property_ReturnsPropertyInfo()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Property));
            var member = Member.FromPropertyInfo(propertyInfo);
            Assert.That(member.MemberSource, Is.EqualTo(propertyInfo));
        }

        [Test]
        public void MemberSource_Field_ReturnsFieldInfo()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.Field));
            var member = Member.FromFieldInfo(fieldInfo);
            Assert.That(member.MemberSource, Is.EqualTo(fieldInfo));
        }

        [Test]
        public void Equals_IsValueEqual()
        {
            var member1 = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));
            var member2 = Member.FromPropertyInfo(typeof(TestClass).GetProperty(nameof(TestClass.Property)));

            Assert.That(member1, Is.Not.SameAs(member2));
            Assert.That(member1, Is.EqualTo(member2));
            Assert.That(member1.GetHashCode(), Is.EqualTo(member2.GetHashCode()));
            Assert.That(member1 == member2);
        }

        private class TestClass
        {
            public int Field;

            public bool Throw { get; set; }

            private string _property;

            public string Property
            {
                get => Throw ? throw new Exception("oops") : _property;
                set => _property = value;
            }
        }
    }
}
