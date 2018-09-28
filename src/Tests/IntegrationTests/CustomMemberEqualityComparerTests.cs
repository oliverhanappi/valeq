using System;
using NUnit.Framework;
using Valeq.Comparers;
using Valeq.Configuration;
using Valeq.Metadata;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class CustomMemberEqualityComparerTests
    {
        [Test]
        public void TakesCustomEqualityComparerForSpecifiedMemberIntoAccount()
        {
            var x = new TestPoco {Custom1 = new CustomPoco {Value = 42}, Custom2 = new CustomPoco {Value = 3}};
            var y = new TestPoco {Custom1 = new CustomPoco {Value = -42}, Custom2 = new CustomPoco {Value = 3}};
            var z = new TestPoco {Custom1 = new CustomPoco {Value = -42}, Custom2 = new CustomPoco {Value = -3}};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }

        private class TestPoco : ValueEquatable
        {
            [EqualityComparer(typeof(CustomPocoEqualityComparer))]
            public CustomPoco Custom1 { get; set; }

            public CustomPoco Custom2 { get; set; }
        }

        private struct CustomPoco
        {
            public int Value { get; set; }
        }

        private class CustomPocoEqualityComparer : GenericEqualityComparer<CustomPoco>
        {
            protected override bool EqualsInternal(CustomPoco x, CustomPoco y)
            {
                return Math.Abs(x.Value) == Math.Abs(y.Value);
            }

            protected override int GetHashCodeInternal(CustomPoco obj)
            {
                return Math.Abs(obj.Value);
            }
        }
    }
}
