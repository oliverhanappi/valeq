using System.Collections.Generic;
using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Metadata;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class SetTests
    {
        [Test]
        public void ComparesAsSet()
        {
            var x = new SetPoco {Values = new[] {1, 2, 3}};
            var y = new SetPoco {Values = new List<int> {2, 1, 3, 3}};
            var z = new SetPoco {Values = new HashSet<int> {1, 2}};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }
        
        private class SetPoco : ValueEquatable
        {
            [Set]
            public IEnumerable<int> Values { get; set; }
        }
    }
}