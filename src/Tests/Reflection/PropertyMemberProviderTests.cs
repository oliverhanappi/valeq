using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Metadata;
using Valeq.Runtime;
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
        public void Class_OnlyPublic_ReturnsAllPublicProperties()
        {
            var members = GetMembers(typeof(BaseClass), PropertySearchScope.OnlyPublic);
            
            Assert.That(members, Has.Count.EqualTo(1));
            Assert.That(members[0].Name, Does.EndWith(nameof(BaseClass.PublicProperty)));
        }
        
        [Test]
        public void Class_All_ReturnsAllProperties()
        {
            var members = GetMembers(typeof(BaseClass), PropertySearchScope.All);
            
            Assert.That(members, Has.Count.EqualTo(3));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(string)));
            Assert.That(members[1].MemberType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Class_OnlyPublic_ReturnsAllPublicPropertiesOfBaseClassesAsWell()
        {
            var members = GetMembers(typeof(SubClass), PropertySearchScope.OnlyPublic);
            Assert.That(members, Has.Count.EqualTo(2));
        }

        [Test]
        public void Class_HandlesHiddenProperties()
        {
            var members = GetMembers(typeof(SubClass), PropertySearchScope.OnlyPublic);

            var subClass = new SubClass {PublicProperty = "sub"};
            ((BaseClass) subClass).PublicProperty = "base";

            Assert.That(members[0].GetValue(subClass), Is.EqualTo("base"));
            Assert.That(members[1].GetValue(subClass), Is.EqualTo("sub"));
        }

        [Test]
        public void Class_All_ReturnsAllPropertiesOfBaseClassesAsWell()
        {
            var members = GetMembers(typeof(SubClass), PropertySearchScope.All);
            Assert.That(members, Has.Count.EqualTo(5));
        }

        [Test]
        public void Class_BaseProperty_ReturnsReflectedVersion()
        {
            var member = GetMembers(typeof(SubClass), PropertySearchScope.All)
                .Single(m => m.Name.EndsWith("ProtectedProperty"));
            
            var customAttributes = ((PropertyInfo) member.MemberSource).GetCustomAttributes(inherit: true);
            Assert.That(customAttributes, Has.Some.InstanceOf<EqualityComparerAttribute>());
        }

        [Test]
        public void Class_MemberGetValue_ReturnsValue()
        {
            var member = GetMembers(typeof(SubClass), PropertySearchScope.OnlyPublic)
                .Single(n => n.Name.Contains(nameof(SubClass)) && n.Name.EndsWith(nameof(SubClass.PublicProperty)));

            var sub = new SubClass {PublicProperty = 123};
            var value = member.GetValue(sub);

            Assert.That(value, Is.EqualTo(123));
        }

        [Test]
        public void Class_BaseClassMemberGetValue_ReturnsValue()
        {
            var member = GetMembers(typeof(BaseClass), PropertySearchScope.All)
                .Single(n => n.Name.EndsWith("PrivateProperty"));

            var sub = new SubClass();
            var value = member.GetValue(sub);
            Assert.That(value, Is.EqualTo("Hello"));
        }
        
        [Test]
        public void Struct_OnlyPublic_ReturnsAllPublicProperties()
        {
            var members = GetMembers(typeof(TestStruct), PropertySearchScope.OnlyPublic);
            
            Assert.That(members, Has.Count.EqualTo(1));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(object)));
        }
        
        [Test]
        public void Struct_All_ReturnsAllProperties()
        {
            var members = GetMembers(typeof(TestStruct), PropertySearchScope.All);
            
            Assert.That(members, Has.Count.EqualTo(2));
            Assert.That(members[0].MemberType, Is.EqualTo(typeof(string)));
            Assert.That(members[1].MemberType, Is.EqualTo(typeof(object)));
        }

        [Test]
        public void Struct_MemberGetValue_ReturnsValue()
        {
            var member = GetMembers(typeof(TestStruct), PropertySearchScope.OnlyPublic)
                .Single(n => n.Name.EndsWith(nameof(TestStruct.PublicProperty)));

            var testStruct = new TestStruct {PublicProperty = 123};
            var value = member.GetValue(testStruct);

            Assert.That(value, Is.EqualTo(123));
        }

        private IReadOnlyList<Member> GetMembers(Type type, PropertySearchScope propertySearchScope)
        {
            var scope = EqualityComparerScope.ForType(type);
            var configuration = new ValueEqualityConfigurationBuilder {DefaultPropertySearchScope = propertySearchScope}.Build();
            var context = new EqualityComparerContext(scope, MetadataCollection.Empty, configuration);

            return _memberProvider.GetMembers(context).OrderBy(m => m.Name).ToList();
        }
        
#pragma warning disable 169, 414, 649

        private interface ITestInterface
        {
            object PublicProperty { get; }
        }

        private abstract class BaseClass
        {
            private string PrivateProperty { get; } = "Hello";
            protected internal virtual int ProtectedProperty { get; }
            public virtual object PublicProperty { get; set; }
        }

        private class SubClass : BaseClass, ITestInterface
        {
            public new object PublicProperty { get; set; }
            private bool PrivateProperty { get; }

            [EqualityComparer(typeof(AbsoluteIntegerEqualityComparer))]
            protected internal override int ProtectedProperty => base.ProtectedProperty;
        }

        private struct TestStruct : ITestInterface
        {
            private string PrivateProperty { get; }
            public object PublicProperty { get; set; }
        }
    }
}
