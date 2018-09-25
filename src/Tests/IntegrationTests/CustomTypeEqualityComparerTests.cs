using System;
using System.Collections.Generic;
using NUnit.Framework;
using Valeq.Comparers;
using Valeq.Configuration;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class CustomTypeEqualityComparerTests
    {
        [Test]
        public void TakesCustomEqualityComparerIntoAccount()
        {
            var x = new TestPoco {CustomEqualityPoco = new CustomEqualityPoco {Value = 42}};
            var y = new TestPoco {CustomEqualityPoco = new CustomEqualityPoco {Value = -42}};
            var z = new TestPoco {CustomEqualityPoco = new CustomEqualityPoco {Value = 27}};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }

        private class TestPoco : ValueEquatable
        {
            public CustomEqualityPoco CustomEqualityPoco { get; set; }
        }

        [EqualityComparer(typeof(CustomEqualityPocoEqualityComparer))]
        private struct CustomEqualityPoco
        {
            public int Value { get; set; }
        }

        private class CustomEqualityPocoEqualityComparer : GenericEqualityComparer<CustomEqualityPoco>
        {
            protected override bool EqualsInternal(CustomEqualityPoco x, CustomEqualityPoco y)
            {
                return Math.Abs(x.Value) == Math.Abs(y.Value);
            }

            protected override int GetHashCodeInternal(CustomEqualityPoco obj)
            {
                return Math.Abs(obj.Value);
            }
        }
    }
}
