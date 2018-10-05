using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Metadata;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    [TestFixture]
    public class FieldMemberProviderTests
    {
        private FieldMemberProvider _memberProvider;

        [SetUp]
        public void SetUp()
        {
            _memberProvider = new FieldMemberProvider();
        }

        [Test]
        public void Null_ThrowsException()
        {
            Assert.That(() => _memberProvider.GetMembers(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Class_ReturnsAllFields()
        {
            var members = GetMembers(typeof(BaseClass));

            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(string)));
            Assert.That(members[1].MemberType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Class_ReturnsAllFieldsOfBaseClassesAsWell()
        {
            var members = GetMembers(typeof(SubClass));
            Assert.That(members, Has.Count.EqualTo(4));
        }

        [Test]
        public void Class_MemberGetValue_ReturnsValue()
        {
            var member = GetMembers(typeof(SubClass)).Single(n => n.Name.EndsWith(nameof(SubClass.Value3)));

            var sub = new SubClass {Value3 = 123};
            var value = member.GetValue(sub);

            Assert.That(value, Is.EqualTo(123));
        }

        [Test]
        public void Class_BaseClassMemberGetValue_ReturnsValue()
        {
            var member = GetMembers(typeof(SubClass)).Single(n => n.Name.EndsWith("_value1"));

            var sub = new SubClass();
            var value = member.GetValue(sub);

            Assert.That(value, Is.EqualTo("Hello"));
        }
        
        [Test]
        public void Class_DoesNotReturnUndiscoverableField()
        {
            var members = GetMembers(typeof(BaseClass));

            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members, Has.None.Matches<Member>(m => m.Name.EndsWith(nameof(BaseClass.Secret))));
        }

        [Test]
        public void Struct_ReturnsAllFields()
        {
            var members = GetMembers(typeof(TestStruct));

            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(string)));
            Assert.That(members[1].MemberType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Struct_MemberGetValue_ReturnsValue()
        {
            var member = GetMembers(typeof(TestStruct)).Single(n => n.Name.EndsWith(nameof(TestStruct.Value2)));

            var testStruct = new TestStruct {Value2 = 123};
            var value = member.GetValue(testStruct);

            Assert.That(value, Is.EqualTo(123));
        }
        
        [Test]
        public void Struct_DoesNotReturnUndiscoverableField()
        {
            var members = GetMembers(typeof(TestStruct));

            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members, Has.None.Matches<Member>(m => m.Name.EndsWith(nameof(BaseClass.Secret))));
        }

        private IReadOnlyList<Member> GetMembers(Type type)
        {
            var scope = EqualityComparerScope.ForType(type);
            var configuration = ValueEqualityConfiguration.CreateDefaultConfiguration();
            var context = new EqualityComparerContext(scope, MetadataCollection.Empty, configuration);

            return _memberProvider.GetMembers(context).OrderBy(m => m.Name).ToList();
        }

#pragma warning disable 169, 414, 649

        private abstract class BaseClass
        {
            private string _value1 = "Hello";
            protected int Value2;

            [UndiscoverableMember]
            public object Secret;
        }

        private interface ITestInterface
        {
        }

        private class SubClass : BaseClass, ITestInterface
        {
            public object Value3;
            private bool Value4;
        }

        private struct TestStruct : ITestInterface
        {
            private string _value1;
            public int Value2;

            [UndiscoverableMember]
            public object Secret;
        }
    }
}
