using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Valeq.Comparers
{
    public static class DefaultEqualityComparer
    {
        private static readonly MethodInfo GetForGenericMethodDefinition;

        static DefaultEqualityComparer()
        {
            GetForGenericMethodDefinition = typeof(DefaultEqualityComparer).GetMethod(nameof(GetFor), BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static IEqualityComparer GetForType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var genericMethod = GetForGenericMethodDefinition.MakeGenericMethod(type);
            return (IEqualityComparer) genericMethod.Invoke(null, new object[0]);
        }

        private static IEqualityComparer GetFor<T>()
        {
            return EqualityComparer<T>.Default;
        }
    }
}
