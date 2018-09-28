using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Valeq;
using Valeq.Runtime;

[assembly: ResetEnvironment]
    
namespace Valeq
{
    public class ResetEnvironmentAttribute : TestActionAttribute
    {
        public override ActionTargets Targets => ActionTargets.Test;

        public override void BeforeTest(ITest test)
        {
            ValueEqualityComparerProvider.ResetCurrentAccessStrategy();
            ValueEqualityComparerProvider.Current = ValueEqualityComparerProvider.CreateDefault();
        }
    }
}
