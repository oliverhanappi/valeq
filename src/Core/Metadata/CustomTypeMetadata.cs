using System;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class CustomTypeMetadata : CustomMetadata
    {
        public Type Type { get; }
        public bool Inherit { get; }

        public CustomTypeMetadata(Type type, IMetadata metadata, bool inherit) : base(metadata)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Inherit = inherit;
        }

        public bool AppliesTo(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (Inherit)
                return type.IsAssignableTo(Type);

            if (Type.IsGenericTypeDefinition && type.IsGenericType && type.GetGenericTypeDefinition() == Type)
                return true;

            return type == Type;
        }
    }
}
