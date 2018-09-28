using NUnit.Framework;
using Valeq.Runtime;

namespace Valeq.Configuration
{
    [TestFixture]
    public class ValueEqualityConfigurationBuilderTests
    {
        [Test]
        public void TakesCustomMetadataIntoAccount()
        {
            var builder = new ValueEqualityConfigurationBuilder();

            builder.Compare.Type<TestPoco>()
                .Property(x => x.Ignored).Ignore()
                .Property(x => x.Value).IgnoreCase();

            builder.Compare.Type<TestSubPoco>()
                .ByReference();

            var configuration = builder.Build();
            var equalityComparerProvider = new ValueEqualityComparerProvider(configuration);
            var equalityComparer = equalityComparerProvider.GetEqualityComparer<TestPoco>();

            var testSubPoco1 = new TestSubPoco();
            var testSubPoco2 = new TestSubPoco();
            
            var testPoco1 = new TestPoco {Value = "Hello World", Ignored = 42, SubPoco = testSubPoco1};
            var testPoco2 = new TestPoco {Value = "HELLO world", Ignored = 27, SubPoco = testSubPoco1};
            var testPoco3 = new TestPoco {Value = "Hallo Welt", Ignored = 42, SubPoco = testSubPoco1};
            var testPoco4 = new TestPoco {Value = "HELLO world", Ignored = 42, SubPoco = testSubPoco2};

            Assert.That(testPoco1, Is.EqualTo(testPoco2).Using(equalityComparer));
            Assert.That(testPoco1, Is.Not.EqualTo(testPoco3).Using(equalityComparer));
            Assert.That(testPoco1, Is.Not.EqualTo(testPoco4).Using(equalityComparer));
        }

        private class TestPoco
        {
            public string Value { get; set; }
            public int Ignored { get; set; }

            public TestSubPoco SubPoco { get; set; }
        }

        private class TestSubPoco
        {
        }
    }
}
