using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Valeq.Metadata;
using Valeq.TestInfrastructure;

namespace Valeq.Reflection
{
    [TestFixture]
    public class PropertyMemberProviderTests
    {
        private PropertyMemberProvider _memberProvider;

        [SetUp]
        public void SetUp()
        {
            _memberProvider = new PropertyMemberProvider();
        }
        
        [Test]
        public void Null_ThrowsException()
        {
            Assert.That(() => _memberProvider.GetMembers(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void Class_ReturnsAllPropertys()
        {
            var members = _memberProvider.GetMembers(typeof(BaseClass))
                .OrderBy(m => m.Name)
                .ToList();
            
            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(string)));
            Assert.That(members[1].MemberType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Class_ReturnsAllPropertysOfBaseClassesAsWell()
        {
            var members = _memberProvider.GetMembers(typeof(SubClass))
                .OrderBy(m => m.Name)
                .ToList();
            
            Assert.That(members, Has.Count.EqualTo(4));
        }

        [Test]
        public void Class_BaseProperty_ReturnsReflectedVersion()
        {
            var member = _memberProvider.GetMembers(typeof(SubClass)).Single(m => m.Name.EndsWith("2"));
            var customAttributes = ((PropertyInfo) member.MemberSource).GetCustomAttributes(inherit: true);

            Assert.That(customAttributes, Has.Some.InstanceOf<EqualityComparerAttribute>());
        }

        [Test]
        public void Class_MemberGetValue_ReturnsValue()
        {
            var member = _memberProvider
                .GetMembers(typeof(SubClass))
                .Single(n => n.Name.EndsWith(nameof(SubClass.Value3)));

            var sub = new SubClass {Value3 = 123};
            var value = member.GetValue(sub);

            Assert.That(value, Is.EqualTo(123));
        }

        [Test]
        public void Class_BaseClassMemberGetValue_ReturnsValue()
        {
            var member = _memberProvider
                .GetMembers(typeof(SubClass))
                .Single(n => n.Name.EndsWith("_value1"));

            var sub = new SubClass();
            var value = member.GetValue(sub);

            Assert.That(value, Is.EqualTo("Hello"));
        }
        
        [Test]
        public void Struct_ReturnsAllPropertys()
        {
            var members = _memberProvider.GetMembers(typeof(TestStruct))
                .OrderBy(m => m.Name)
                .ToList();
            
            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(string)));
            Assert.That(members[1].MemberType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Struct_MemberGetValue_ReturnsValue()
        {
            var member = _memberProvider
                .GetMembers(typeof(TestStruct))
                .Single(n => n.Name.EndsWith(nameof(TestStruct.Value3)));

            var testStruct = new TestStruct {Value3 = 123};
            var value = member.GetValue(testStruct);

            Assert.That(value, Is.EqualTo(123));
        }
        
#pragma warning disable 169, 414, 649

        private interface ITestInterface
        {
            object Value3 { get; }
        }

        private abstract class BaseClass
        {
            private string _value1 { get; } = "Hello";
            protected virtual int Value2 { get; }
        }

        private class SubClass : BaseClass, ITestInterface
        {
            public object Value3 { get; set; }
            private bool Value4 { get; }

            [EqualityComparer(typeof(AbsoluteIntegerEqualityComparer))]
            protected override int Value2 => base.Value2;
        }

        private struct TestStruct : ITestInterface
        {
            private string _value1 { get; }
            public object Value3 { get; set; }
        }
    }
}
