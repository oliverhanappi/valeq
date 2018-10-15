using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Valeq.Runtime;
using Valeq.TestDataBuilders;

namespace Valeq.Reflection
{
    [TestFixture]
    public class CachingMemberProviderDecoratorTests
    {
        private EqualityComparerContext _context;
        private IMemberProvider _memberProvider;
        private CachingMemberProviderDecorator _decorator;

        [SetUp]
        public void SetUp()
        {
            _context = Create.An.EqualityComparerContext().Build();
            _memberProvider = A.Fake<IMemberProvider>(o => o.Strict());
            _decorator = new CachingMemberProviderDecorator(_memberProvider);
        }

        [Test]
        public void Ctor_NoInner_ThrowsException()
        {
            Assert.That(() => new CachingMemberProviderDecorator(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetMembers_NoContext_ThrowsException()
        {
            Assert.That(() => _decorator.GetMembers(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetMembers_ReturnsInnerResult()
        {
            var members = new List<Member> {new Member("Test", this, typeof(object), _ => null, t => true)};
            A.CallTo(() => _memberProvider.GetMembers(_context)).Returns(members);

            var result = _decorator.GetMembers(_context);
            Assert.That(result, Is.EqualTo(members));
        }

        [Test]
        public void GetMembers_CallsInnerOnlyOnce()
        {
            A.CallTo(() => _memberProvider.GetMembers(_context)).Returns(new List<Member>()).Once();

            var result1 = _decorator.GetMembers(_context);
            var result2 = _decorator.GetMembers(_context);
            Assert.That(result2, Is.SameAs(result1));

            A.CallTo(() => _memberProvider.GetMembers(_context)).MustHaveHappenedOnceExactly();
        }
    }
}
