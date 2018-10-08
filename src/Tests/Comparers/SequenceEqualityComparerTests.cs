using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Valeq.TestInfrastructure;

namespace Valeq.Comparers
{
    [TestFixture]
    public class SequenceEqualityComparerTests
    {
        private SequenceEqualityComparer<int> _sequenceEqualityComparer;

        [SetUp]
        public void SetUp()
        {
            _sequenceEqualityComparer = new SequenceEqualityComparer<int>(new AbsoluteIntegerEqualityComparer());
        }

        [Test]
        public void Ctor_NoElementEqualityComparer_ThrowsException()
        {
            Assert.That(() => new SequenceEqualityComparer<object>(null), Throws.ArgumentNullException);
        }
        
        public static IEnumerable<TestCaseData> EqualsTestCases
        {
            get
            {
                yield return new TestCaseData(null, null) { TestName = "both null ->  equal", ExpectedResult = true};
                yield return new TestCaseData(null, "") { TestName = "null and empty -> not equal", ExpectedResult = false};
                yield return new TestCaseData("", null) { TestName = "empty and null -> not equal", ExpectedResult = false};
                yield return new TestCaseData("", "") { TestName = "both empty -> equal", ExpectedResult = true};
                yield return new TestCaseData("1", "-1") { TestName = "applies element comparer", ExpectedResult = true};
                yield return new TestCaseData("1, 1", "1") { TestName = "different count -> not equal", ExpectedResult = false};
                yield return new TestCaseData("1, 2", "1, 2") { TestName = "same elements same order -> equal", ExpectedResult = true};
                yield return new TestCaseData("1, 2", "2, 1") { TestName = "same elements other order -> equal", ExpectedResult = false};
            }
        }

        [TestCaseSource(nameof(EqualsTestCases))]
        public bool Equals(string specificationX, string specificationY)
        {
            var x = IntegerList.Parse(specificationX)?.ProtectAgainstMultipleEnumeration();
            var y = IntegerList.Parse(specificationY)?.ProtectAgainstMultipleEnumeration();

            return _sequenceEqualityComparer.Equals(x, y);
        }

        public static IEnumerable<TestCaseData> HashCodeTestCases => EqualsTestCases
            .Where(tc => Equals(tc.ExpectedResult, true))
            .Select(tc => new TestCaseData(tc.Arguments) {TestName = "GetHashCode: " + tc.TestName.Replace(" -> equal", String.Empty)});

        [TestCaseSource(nameof(HashCodeTestCases))]
        public void GetHashCode_ReturnsSameHashCodeForEqualCollections(string specificationX, string specificationY)
        {
            var x = IntegerList.Parse(specificationX)?.ProtectAgainstMultipleEnumeration();
            var y = IntegerList.Parse(specificationY)?.ProtectAgainstMultipleEnumeration();

            var hashCodeX = _sequenceEqualityComparer.GetHashCode(x);
            var hashCodeY = _sequenceEqualityComparer.GetHashCode(y);
            
            Assert.That(hashCodeX, Is.EqualTo(hashCodeY));
        }

        [Test]
        public void Create_CreatesEqualityComparer()
        {
            var equalityComparer = SequenceEqualityComparer.Create(typeof(int), new AbsoluteIntegerEqualityComparer());
            Assert.That(equalityComparer, Is.InstanceOf<SequenceEqualityComparer<int>>());
            Assert.That(equalityComparer.Equals(new[]{1}, new[]{-1}), Is.True);
        }

        [Test]
        public void Create_NoType_ThrowsException()
        {
            Assert.That(() => SequenceEqualityComparer.Create(null, new AbsoluteIntegerEqualityComparer()),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Create_NoElementEqualityComparer_ThrowsException()
        {
            Assert.That(() => SequenceEqualityComparer.Create(typeof(int), null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void Create_WrongElementEqualityComparer_ThrowsException()
        {
            Assert.That(() => SequenceEqualityComparer.Create(typeof(string), new AbsoluteIntegerEqualityComparer()),
                Throws.ArgumentException.With.Message.Contains("IEqualityComparer<System.String>"));
        }
    }
}
