using System;
using System.Linq;
using NUnit.Framework;
using Valeq.Reflection;
using Valeq.TestInfrastructure;

namespace Valeq.Comparers
{
    [TestFixture]
    public class MemberEqualityComparerTests
    {
        private MemberEqualityComparer<TestPoco> _noMembersEqualityComparer;
        private MemberEqualityComparer<TestPoco> _memberEqualityComparer;

        [SetUp]
        public void SetUp()
        {
            _noMembersEqualityComparer =
                new MemberEqualityComparer<TestPoco>(Enumerable.Empty<MemberComparisonConfiguration>());

            var member1 = new Member(nameof(TestPoco.Value1), new object(), typeof(int),
                o => ((TestPoco) o).Value1, t => t == typeof(TestPoco));

            var member2 = new Member(nameof(TestPoco.Value2), new object(), typeof(int), o =>
            {
                var testPoco = (TestPoco) o;
                if (testPoco.ThrowOnAccessingValue2)
                    throw new Exception("invalid access");

                return testPoco.Value2;
            }, t => t == typeof(TestPoco));

            _memberEqualityComparer = new MemberEqualityComparer<TestPoco>(new[]
            {
                new MemberComparisonConfiguration(member1, new AbsoluteIntegerEqualityComparer()),
                new MemberComparisonConfiguration(member2, DefaultEqualityComparer.GetForType(typeof(int))),
            });
        }

        [TestCase(1, 1, 1, 1, ExpectedResult = true)]
        [TestCase(1, 1, -1, 1, ExpectedResult = true)]
        [TestCase(1, 2, 1, 1, ExpectedResult = false)]
        [TestCase(1, 1, 2, 1, ExpectedResult = false)]
        public bool Equals(int valueX1, int valueX2, int valueY1, int valueY2)
        {
            var x = new TestPoco {Value1 = valueX1, Value2 = valueX2};
            var y = new TestPoco {Value1 = valueY1, Value2 = valueY2};

            return _memberEqualityComparer.Equals(x, y);
        }

        [Test]
        public void Equals_ExceptionOnMemberAccess_ThrowsException()
        {
            var x = new TestPoco {Value1 = 1, Value2 = 2};
            var y = new TestPoco {Value1 = 1, ThrowOnAccessingValue2 = true};

            Assert.That(() => _memberEqualityComparer.Equals(x, y), Throws.InstanceOf<MemberRetrievalException>()
                .With.Message.Contains(nameof(TestPoco.Value2)));
        }

        [Test]
        public void Equals_StopsComparisonAtFirstNonEqualMember()
        {
            var x = new TestPoco {Value1 = 1};
            var y = new TestPoco {Value1 = 2, ThrowOnAccessingValue2 = true};

            Assert.That(x, Is.Not.EqualTo(y).Using<TestPoco>(_memberEqualityComparer));
        }

        [Test]
        public void NoMembers_AlwaysEqual()
        {
            var testPoco1 = new TestPoco {Value1 = 42};
            var testPoco2 = new TestPoco {Value1 = 27};

            Assert.That(testPoco1, Is.EqualTo(testPoco2).Using<TestPoco>(_noMembersEqualityComparer));
        }

        [Test]
        public void NoMembers_GetHashCode_ReturnsZero()
        {
            var hashCode = _noMembersEqualityComparer.GetHashCode(new TestPoco());
            Assert.That(hashCode, Is.EqualTo(0));
        }

        [Test]
        public void Ctor_NullMembers_ThrowsException()
        {
            Assert.That(() => new MemberEqualityComparer<object>(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetHashCode_EqualInstancesReturnSameHashcode()
        {
            var x = new TestPoco {Value1 = 1, Value2 = 1};
            var y = new TestPoco {Value1 = -1, Value2 = 1};

            var hashCodeX = _memberEqualityComparer.GetHashCode(x);
            var hashCodeY = _memberEqualityComparer.GetHashCode(y);

            Assert.That(hashCodeX, Is.EqualTo(hashCodeY));
        }

        private class TestPoco
        {
            public int Value1 { get; set; }
            public int Value2 { get; set; }

            public bool ThrowOnAccessingValue2 { get; set; }
        }
    }
}
