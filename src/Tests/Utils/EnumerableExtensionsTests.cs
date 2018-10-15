using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Valeq.TestInfrastructure;

namespace Valeq.Utils
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void TryGetCount_Collection_ReturnsCount()
        {
            var result = new List<int> {1, 2, 3}.TryGetCount(out var count);
            Assert.That(result, Is.True);
            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void TryGetCount_Enumerable_ReturnsFalse()
        {
            var result = new List<int> {1, 2, 3}.Where(i => i > 0).TryGetCount(out _);
            Assert.That(result, Is.False);
        }

        [Test]
        public void FullOuterJoin()
        {
            var leftValues = new[]
            {
                new {LeftId = 1, Value = "Left1"},
                new {LeftId = 2, Value = "Left2"}
            };

            var rightValues = new[]
            {
                new {RightId = -2, Value = "Right2"},
                new {RightId = -3, Value = "Right3"}
            };

            var joinResult = leftValues
                .FullOuterJoin(rightValues, l => l.LeftId, r => r.RightId,
                    (key, left, right) => new {Key = key, Left = left, Right = right},
                    new {LeftId = 100, Value = "LeftMissing"}, new {RightId = 200, Value = "RightMissing"},
                    new AbsoluteIntegerEqualityComparer())
                .OrderBy (r => r.Key)
                .ToList();

            Assert.That(joinResult, Is.EquivalentTo(new[]
            {
                new {Key = 1, Left = new {LeftId = 1, Value = "Left1"}, Right = new {RightId = 200, Value = "RightMissing"}},
                new {Key = 2, Left = new {LeftId = 2, Value = "Left2"}, Right = new {RightId = -2, Value = "Right2"}},
                new {Key = -3, Left = new {LeftId = 100, Value = "LeftMissing"}, Right = new {RightId = -3, Value = "Right3"}},
            }));
        }
    }
}
