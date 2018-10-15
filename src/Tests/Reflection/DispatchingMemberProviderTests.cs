using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Valeq.Metadata;
using Valeq.TestDataBuilders;

namespace Valeq.Reflection
{
    [TestFixture]
    public class DispatchingMemberProviderTests
    {
        [Test]
        public void Ctor_Null_ThrowsException()
        {
            Assert.That(() => new DispatchingMemberProvider(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetMembers_NullContext_ThrowsException()
        {
            var memberProvider =
                new DispatchingMemberProvider(new CustomMetadataProvider(Enumerable.Empty<CustomMetadata>()));
            
            Assert.That(() => memberProvider.GetMembers(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetMembers_Interface_ReturnsPropertyMembers()
        {
            AssertPropertyMembers<ITestInterface>(expectedCount: 2);
        }

        [Test]
        public void GetMembers_TypeWithoutAnyMetadata_ReturnsFieldMembers()
        {
            AssertFieldMembers<TestPoco>(expectedCount: 1);
        }

        [Test]
        public void GetMembers_TypeWithPropertyMetadata_ReturnsPropertyMembers()
        {
            var member = Member.FromPropertyInfo(typeof(TestPoco).GetProperty(nameof(TestPoco.Value)));
            AssertPropertyMembers<TestPoco>(2, new CustomMemberMetadata(member, new DefaultEqualityAttribute()));
        }

        [Test]
        public void GetMembers_TypeWithFieldMetadata_ReturnsFieldMembers()
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var member = Member.FromFieldInfo(typeof(TestPoco).GetFields(bindingFlags).Single());
            AssertFieldMembers<TestPoco>(1, new CustomMemberMetadata(member, new DefaultEqualityAttribute()));
        }

        [Test]
        public void GetMembers_TypeWithPropertyAndFieldMetadata_ThrowsException()
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var fieldMember = Member.FromFieldInfo(typeof(TestPoco).GetFields(bindingFlags).Single());
            var propertyMember = Member.FromPropertyInfo(typeof(TestPoco).GetProperty(nameof(TestPoco.Value)));

            var metadatas = new CustomMetadata[]
            {
                new CustomMemberMetadata(fieldMember, new DefaultEqualityAttribute()),
                new CustomMemberMetadata(propertyMember, new DefaultEqualityAttribute())
            };

            Assert.That(() => GetMembers<TestPoco>(metadatas), Throws.ArgumentException
                .With.Message.Contains("Unable to choose between comparing fields or properties"));
        }

        private void AssertFieldMembers<T>(int expectedCount, params CustomMetadata[] metadatas)
        {
            var members = GetMembers<T>(metadatas);
            Assert.That(members, Has.Count.EqualTo(expectedCount));
            Assert.That(members, Has.All.Matches<Member>(m => m.MemberSource is FieldInfo));
        }

        private void AssertPropertyMembers<T>(int expectedCount, params CustomMetadata[] metadatas)
        {
            var members = GetMembers<T>(metadatas);
            Assert.That(members, Has.Count.EqualTo(expectedCount));
            Assert.That(members, Has.All.Matches<Member>(m => m.MemberSource is PropertyInfo));
        }

        private IReadOnlyList<Member> GetMembers<T>(params CustomMetadata[] metadatas)
        {
            var metadataProvider = new CustomMetadataProvider(metadatas);
            var memberProvider = new DispatchingMemberProvider(metadataProvider);
            
            var context = Create.An.EqualityComparerContext().ForType<T>();
            return memberProvider.GetMembers(context).OrderBy(m => m.Name).ToList();
        }

        private interface ITestInterface
        {
            string Value1 { get; }
            int Value2 { get; set; }
        }

        private class TestPoco
        {
            public string Value { get; set; }
            public string TransientValue => Value;
        }
    }
}
