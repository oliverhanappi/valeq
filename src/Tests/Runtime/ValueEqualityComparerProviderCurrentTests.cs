using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Valeq.Configuration;

namespace Valeq.Runtime
{
    [TestFixture]
    public class ValueEqualityComparerProviderCurrentTests
    {
        [Test]
        public void Current_IsStatic()
        {
            var provider = ValueEqualityComparerProvider.CreateDefault();

            var thread = new Thread(() => ValueEqualityComparerProvider.Current = provider);
            thread.Start();
            thread.Join();

            Assert.That(ValueEqualityComparerProvider.Current, Is.SameAs(provider));
        }

        [Test]
        public void Current_IsAlwaysInitialized()
        {
            Assert.That(ValueEqualityComparerProvider.Current, Is.Not.Null);
        }

        [Test]
        public void Current_NullReturnedFromGetter_CreatesDefault()
        {
            IValueEqualityComparerProvider provider = null;
            ValueEqualityComparerProvider.SetCurrentAccessStrategy(() => provider, v => provider = v);

            var current = ValueEqualityComparerProvider.Current;

            Assert.That(current, Is.Not.Null);
            Assert.That(provider, Is.Not.Null);
            Assert.That(current, Is.SameAs(provider));
        }
        
        [Test]
        public void Current_NullSetter_ThrowsException()
        {
            Assert.That(() => ValueEqualityComparerProvider.Current = null, Throws.ArgumentNullException);
        }

        [Test]
        public async Task SetCurrentAccessStrategy_UsesAccessStrategy()
        {
            var provider = ValueEqualityComparerProvider.CreateDefault();
            var asyncLocal = new AsyncLocal<IValueEqualityComparerProvider>();

            ValueEqualityComparerProvider.SetCurrentAccessStrategy(() => asyncLocal.Value, v => asyncLocal.Value = v);

            Assert.That(ValueEqualityComparerProvider.Current, Is.Not.SameAs(provider));
            await Run();
            Assert.That(ValueEqualityComparerProvider.Current, Is.Not.SameAs(provider));

            async Task Run()
            {
                ValueEqualityComparerProvider.Current = provider;
                await Task.Yield();

                Assert.That(ValueEqualityComparerProvider.Current, Is.SameAs(provider));
            }
        }

        [TestCase(false, true)]
        [TestCase(true, true)]
        [TestCase(true, false)]
        public void SetCurrentAccessStrategy_Null_ThrowsException(bool nullGetter, bool nullSetter)
        {
            var getter = !nullGetter ? (Func<IValueEqualityComparerProvider>) (() => null) : null;
            var setter = !nullSetter ? (Action<IValueEqualityComparerProvider>) (_ => { }) : null;

            Assert.That(() => ValueEqualityComparerProvider.SetCurrentAccessStrategy(getter, setter),
                Throws.ArgumentNullException);
        }
        
        [Test]
        public void Configure_SetsCurrent()
        {
            var equalityComparer1 = ValueEqualityComparerProvider.Current.GetEqualityComparer<object>();
            Assert.That(new object(), Is.EqualTo(new object()).Using(equalityComparer1));
            
            ValueEqualityComparerProvider.Configure(b =>
                b.DefaultEqualityComparisonType = EqualityComparisonType.DefaultEquality);

            var equalityComparer2 = ValueEqualityComparerProvider.Current.GetEqualityComparer<object>();
            Assert.That(new object(), Is.Not.EqualTo(new object()).Using(equalityComparer2));
        }
        
        [Test]
        public void Configure_Null_ThrowsException()
        {
            Assert.That(() => ValueEqualityComparerProvider.Configure(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void CreateDefault_DoesNotFail()
        {
            Assert.That(ValueEqualityComparerProvider.CreateDefault, Throws.Nothing);
        }

        [Test]
        public void ResetCurrentAccessStrategy_ResetsToStatic()
        {
            var provider = ValueEqualityComparerProvider.CreateDefault();
            
            ValueEqualityComparerProvider.SetCurrentAccessStrategy(
                () => throw new Exception("getter"), _ => throw new Exception("setter"));

            Assert.That(() => ValueEqualityComparerProvider.Current, Throws.Exception.With.Message.EqualTo("getter"));
            Assert.That(() => ValueEqualityComparerProvider.Current = provider,
                Throws.Exception.With.Message.EqualTo("setter"));
            
            ValueEqualityComparerProvider.ResetCurrentAccessStrategy();

            var thread = new Thread(() => ValueEqualityComparerProvider.Current = provider);
            thread.Start();
            thread.Join();

            Assert.That(ValueEqualityComparerProvider.Current, Is.SameAs(provider));
        }
    }
}
