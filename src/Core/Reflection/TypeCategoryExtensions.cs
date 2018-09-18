using System;
using System.Collections;
using System.Linq;

namespace Valeq.Reflection
{
    public static class TypeCategoryExtensions
    {
        private static readonly Type[] KnownSimpleTypes = {typeof(void), typeof(string), typeof(decimal), typeof(DateTime), typeof(TimeSpan)};

        public static TypeCategory GetCategory(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (KnownSimpleTypes.Contains(type))
                return TypeCategory.Simple;

            if (type.IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null)
                    return underlyingType.GetCategory();

                return type.IsPrimitive || type.IsEnum ? TypeCategory.Simple : TypeCategory.Complex;
            }

            if (typeof(Delegate).IsAssignableFrom(type))
                return TypeCategory.Simple;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return TypeCategory.Collection;

            return TypeCategory.Complex;
        }
    }
}
