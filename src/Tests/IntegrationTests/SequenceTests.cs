using System.Collections.Generic;
using NUnit.Framework;

namespace Valeq.IntegrationTests
{
    [TestFixture]
    public class SequenceTests
    {
        [Test]
        public void ComparesSequenceElementWise()
        {
            var x = new SequencePoco {Values = new[] {1, 2, 3}};
            var y = new SequencePoco {Values = new List<int> {1, 2, 3}};
            var z = new SequencePoco {Values = new HashSet<int> {1, 2}};

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x, Is.Not.EqualTo(z));
        }
        
        private class SequencePoco : ValueEquatable
        {
            public IEnumerable<int> Values { get; set; }
        }
    }
}
