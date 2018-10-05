using System;
using System.Collections.Generic;

namespace Valeq.Reflection
{
    /// <summary>
    /// Sorts types by inheritance hierarchy. Base types are smaller than derived types.
    /// </summary>
    public class TypeComparator : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            if (x == null || y == null || x == y) return 0;
            
            if (x.IsAssignableTo(y)) return 1;
            if (y.IsAssignableTo(x)) return -1;

            return 0;
        }
    }
}