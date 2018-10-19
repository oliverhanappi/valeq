using System;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class MemberInfoExtensionsTests
    {
        [Test]
        public void IsPartOf_NoMember_ThrowsException()
        {
            Assert.That(() => MemberInfoExtensions.IsPartOf(null, typeof(object)), Throws.ArgumentNullException);
        }
        
        [Test]
        public void IsPartOf_NoType_ThrowsException()
        {
            var propertyInfo = typeof(BaseClass).GetProperty(nameof(BaseClass.Property));
            Assert.That(() => propertyInfo.IsPartOf(null), Throws.ArgumentNullException);
        }
        
        [TestCase(typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(SubClass), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Class_Property(Type type)
        {
            var propertyInfo = typeof(BaseClass).GetProperty(nameof(BaseClass.Property));
            return propertyInfo.IsPartOf(type);
        }
        
        [TestCase(typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(SubClass), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Class_OverridableBaseProperty(Type type)
        {
            var propertyInfo = typeof(BaseClass).GetProperty(nameof(BaseClass.OverridableProperty));
            return propertyInfo.IsPartOf(type);
        }
        
        [TestCase(typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(SubClass), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Class_OverridableSubProperty(Type type)
        {
            var propertyInfo = typeof(SubClass).GetProperty(nameof(SubClass.OverridableProperty));
            return propertyInfo.IsPartOf(type);
        }
        
        [TestCase(typeof(BaseClass), ExpectedResult = true)]
        [TestCase(typeof(SubClass), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Class_Field(Type type)
        {
            var propertyInfo = typeof(BaseClass).GetField(nameof(BaseClass.Field));
            return propertyInfo.IsPartOf(type);
        }
        
        [TestCase(typeof(TestStruct), ExpectedResult = true)]
        [TestCase(typeof(ValueType), ExpectedResult = false)]
        public bool IsPartOf_Struct_Property(Type type)
        {
            var propertyInfo = typeof(TestStruct).GetProperty(nameof(TestStruct.Property));
            return propertyInfo.IsPartOf(type);
        }
        
        [TestCase(typeof(TestStruct), ExpectedResult = true)]
        [TestCase(typeof(ValueType), ExpectedResult = false)]
        public bool IsPartOf_Struct_Field(Type type)
        {
            var propertyInfo = typeof(TestStruct).GetField(nameof(TestStruct.Field));
            return propertyInfo.IsPartOf(type);
        }
        
        [TestCase(typeof(IBaseInterface), ExpectedResult = true)]
        [TestCase(typeof(ISubInterface), ExpectedResult = true)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsPartOf_Interface_Property(Type type)
        {
            var propertyInfo = typeof(IBaseInterface).GetProperty(nameof(IBaseInterface.Property));
            return propertyInfo.IsPartOf(type);
        }
        
#pragma warning disable 649        

        private class BaseClass
        {
            public string Property { get; set; }
            public virtual string OverridableProperty { get; set; }
            public string Field;
        }

        private class SubClass : BaseClass
        {
            public override string OverridableProperty
            {
                get => base.OverridableProperty;
                set => base.OverridableProperty = value + "Sub";
            }
        }

        private struct TestStruct
        {
            public string Property { get; set; }
            public string Field;
        }

        private interface IBaseInterface
        {
            string Property { get; set; }
        }

        private interface ISubInterface : IBaseInterface
        {
        }
    }
}
