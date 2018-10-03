using System;
using System.Collections.Generic;
using System.Linq;

namespace Valeq.TestInfrastructure
{
    public static class IntegerList
    {
        public static IReadOnlyList<int> Parse(string specification)
        {
            if (specification == null)
                return null;

            if (String.IsNullOrWhiteSpace(specification))
                return new int[0];

            return specification.Split(',')
                .Where(p => !String.IsNullOrWhiteSpace(p))
                .Select(p => Int32.Parse(p.Trim()))
                .ToList();
        }
    }
}
