using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Valueq.Reflection
{
    public static class DisplayNameExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
            {
                return $"{type.GetElementType().GetDisplayName()}[]";
            }

            if (type.IsGenericType)
            {
                var formattedTypeArguments = String.Join(", ", GetActualGenericParameters().Select(GetDisplayName));
                return $"{GetFullName()}<{formattedTypeArguments}>";
            }

            return GetFullName();

            IEnumerable<Type> GetActualGenericParameters()
            {
                var genericArguments = type.GetGenericArguments();
                if (!type.IsNested)
                    return genericArguments;

                return genericArguments.Skip(type.DeclaringType.GetGenericArguments().Length);
            }

            string GetFullName()
            {
                if (!type.IsNested)
                    return $"{type.Namespace}.{GetName()}";

                var declaringType = type.DeclaringType;

                if (declaringType.IsGenericTypeDefinition)
                {
                    var declaringTypeGenericArguments = type.GetGenericArguments().Take(declaringType.GetGenericArguments().Length);
                    declaringType = declaringType.MakeGenericType(declaringTypeGenericArguments.ToArray());
                }

                return $"{declaringType.GetDisplayName()}.{GetName()}";
            }

            string GetName()
            {
                if (!type.IsGenericType)
                    return type.Name;

                var genericArityIndex = type.Name.IndexOf('`');
                return type.Name.Substring(0, genericArityIndex);
            }
        }

        public static string GetDisplayName(this MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            return $"{memberInfo.DeclaringType.GetDisplayName()}.{memberInfo.Name}{GetSuffix()}";

            string GetSuffix()
            {
                switch (memberInfo)
                {
                    case PropertyInfo propertyInfo:
                        var indexParameters = propertyInfo.GetIndexParameters();
                        if (indexParameters.Length == 0)
                            return String.Empty;

                        return $"[{String.Join(", ", indexParameters.Select(indexParameter => $"{indexParameter.ParameterType.GetDisplayName()} {indexParameter.Name}"))}]";

                    default:
                        return String.Empty;
                }
            }
        }
    }
}
