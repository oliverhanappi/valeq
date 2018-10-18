using System;
using System.Runtime.Serialization;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public class DefaultActivator : IActivator
    {
        public object CreateInstance(Type type, EqualityComparerContext context)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (type.IsValueType)
                return FormatterServices.GetUninitializedObject(type);
            
            if (!type.HasParameterLessConstructor())
                throw new ArgumentException($"{type.GetDisplayName()} has no parameterless constructor.");

            return Activator.CreateInstance(type);
        }
    }
}