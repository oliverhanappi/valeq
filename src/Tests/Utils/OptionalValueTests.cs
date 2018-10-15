using System;
using System.Collections;
using NUnit.Framework;

namespace Valeq.Utils
{
    [TestFixture]
    public class OptionalValueTests
    {
        private OptionalValue<int> _some;
        private OptionalValue<int> _none;

        [SetUp]
        public void SetUp()
        {
            _some = 42;
            _none = OptionalValue<int>.None;
        }

        [Test]
        public void Ctor_Null_ProducesNone()
        {
            var optionalValue = new OptionalValue<object>(null);
            Assert.That(optionalValue.HasValue, Is.False);
        }

        [Test]
        public void Ctor_SomeValue_ProducesSome()
        {
            var optionalValue = new OptionalValue<object>(new object());
            Assert.That(optionalValue.HasValue, Is.True);
        }

        [Test]
        public void Ctor_DefaultNonNullValue_ProducesSome()
        {
            var optionalValue = new OptionalValue<int>(default(int));
            Assert.That(optionalValue.HasValue, Is.True);
        }

        [Test]
        public void HasValue_Some_ReturnsTrue()
        {
            Assert.That(_some.HasValue, Is.True);
        }

        [Test]
        public void HasValue_None_ReturnsFalse()
        {
            Assert.That(_none.HasValue, Is.False);
        }

        [Test]
        public void Match_Some_ExecutesMapperCallback()
        {
            var result = _some.Match(i => i * 10, () => throw new Exception());
            Assert.That(result, Is.EqualTo(420));
        }

        [Test]
        public void Match_Some_MapperCallbackProducesNull_ThrowsException()
        {
            Assert.That(() => _some.Match<object>(i => null, () => throw new Exception()),
                Throws.InvalidOperationException);
        }

        [Test]
        public void Match_None_ExecutesFallbackCallback()
        {
            var result = _none.Match(_ => throw new Exception(), () => 42);
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void Match_None_FallbackCallbackProducesNull_ThrowsException()
        {
            Assert.That(() => _none.Match<object>(_ => throw new Exception(), () => null),
                Throws.InvalidOperationException);
        }

        [Test]
        public void ToString_Some()
        {
            Assert.That(_some.ToString(), Is.EqualTo("Some(42) of System.Int32"));
        }

        [Test]
        public void ToString_None()
        {
            Assert.That(_none.ToString(), Is.EqualTo("None of System.Int32"));
        }

        [TestCaseSource(nameof(EqualityTestCases))]
        public bool Equals_UsesValueEquality(int? x, int? y)
        {
            var optionalValueX = x != null ? new OptionalValue<int>(x.Value) : OptionalValue<int>.None;
            var optionalValueY = y != null ? new OptionalValue<int>(y.Value) : OptionalValue<int>.None;

            return optionalValueX.Equals(optionalValueY);
        }

        [TestCaseSource(nameof(EqualityTestCases))]
        public bool EqualityOperator_UsesValueEquality(int? x, int? y)
        {
            var optionalValueX = x != null ? new OptionalValue<int>(x.Value) : OptionalValue<int>.None;
            var optionalValueY = y != null ? new OptionalValue<int>(y.Value) : OptionalValue<int>.None;

            return optionalValueX == optionalValueY;
        }

        [TestCaseSource(nameof(EqualityTestCases))]
        public bool InEqualityOperator_UsesValueEquality(int? x, int? y)
        {
            var optionalValueX = x != null ? new OptionalValue<int>(x.Value) : OptionalValue<int>.None;
            var optionalValueY = y != null ? new OptionalValue<int>(y.Value) : OptionalValue<int>.None;

            var result = optionalValueX != optionalValueY;
            return !result;
        }

        [TestCase(null, null)]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void GetHashCode_EqualValues_ReturnsEqualHashCode(int? x, int? y)
        {
            var optionalValueX = x != null ? new OptionalValue<int>(x.Value) : OptionalValue<int>.None;
            var optionalValueY = y != null ? new OptionalValue<int>(y.Value) : OptionalValue<int>.None;

            var hashCodeX = optionalValueX.GetHashCode();
            var hashCodeY = optionalValueY.GetHashCode();

            Assert.That(hashCodeX, Is.EqualTo(hashCodeY));
        }

        public static IEnumerable EqualityTestCases
        {
            get
            {
                yield return new TestCaseData(null, null) {ExpectedResult = true};
                yield return new TestCaseData(null, 0) {ExpectedResult = false};
                yield return new TestCaseData(null, 1) {ExpectedResult = false};
                yield return new TestCaseData(0, null) {ExpectedResult = false};
                yield return new TestCaseData(0, 0) {ExpectedResult = true};
                yield return new TestCaseData(0, 1) {ExpectedResult = false};
                yield return new TestCaseData(1, null) {ExpectedResult = false};
                yield return new TestCaseData(1, 0) {ExpectedResult = false};
                yield return new TestCaseData(1, 1) {ExpectedResult = true};
            }
        }
    }
}
