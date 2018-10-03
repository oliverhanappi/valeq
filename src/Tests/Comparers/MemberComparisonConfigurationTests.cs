using System.Reflection;
using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestInfrastructure;

namespace Valeq.Comparers
{
    [TestFixture]
    public class MemberComparisonConfigurationTests
    {
        private int _testMember;

        private readonly AbsoluteIntegerEqualityComparer _equalityComparer = new AbsoluteIntegerEqualityComparer();

        private readonly Member _member = Member.FromFieldInfo(
            typeof(MemberComparisonConfigurationTests).GetField(nameof(_testMember),
                BindingFlags.Instance | BindingFlags.NonPublic));

        [Test]
        public void Ctor_InitializesConfiguration()
        {
            var configuration = new MemberComparisonConfiguration(_member, _equalityComparer);
            Assert.That(configuration.Member, Is.SameAs(_member));
            Assert.That(configuration.EqualityComparer, Is.SameAs(_equalityComparer));
            
            _testMember = 123;
            Assert.That(configuration.Member.GetValue(this), Is.EqualTo(123));
        }
        
        [Test]
        public void Ctor_NoMember_ThrowsException()
        {
            Assert.That(() => new MemberComparisonConfiguration(null, _equalityComparer), Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_NoEqualityComparer_ThrowsException()
        {
            Assert.That(() => new MemberComparisonConfiguration(_member, null), Throws.ArgumentNullException);
        }
    }
}
