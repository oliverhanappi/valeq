using System;
using Valeq.Comparers;

namespace Valeq.TestInfrastructure
{
    public class AbsoluteIntegerEqualityComparer : GenericEqualityComparer<int>
    {
        protected override bool EqualsInternal(int x, int y)
        {
            return Math.Abs(x) == Math.Abs(y);
        }

        protected override int GetHashCodeInternal(int obj)
        {
            return Math.Abs(obj);
        }
    }
}
