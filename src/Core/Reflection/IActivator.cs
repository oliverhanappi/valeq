using System;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public interface IActivator
    {
        object CreateInstance(Type type, EqualityComparerContext context);
    }
}
