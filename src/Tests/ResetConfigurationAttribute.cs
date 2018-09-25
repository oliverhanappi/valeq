using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Valeq;
using Valeq.Configuration;

[assembly: ResetConfiguration]
    
namespace Valeq
{
    public class ResetConfigurationAttribute : TestActionAttribute
    {
        public override ActionTargets Targets => ActionTargets.Test;

        public override void BeforeTest(ITest test)
        {
            ValueEqualityConfiguration.Current = ValueEqualityConfiguration.CreateDefaultConfiguration();
        }
    }
}
