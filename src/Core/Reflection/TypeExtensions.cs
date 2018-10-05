using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Valeq.Reflection
{
    public static class TypeExtensions
    {
        public static bool HasParameterLessConstructor(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(c => c.GetParameters().Length == 0);
        }

        public static bool IsAssignableToAny(this Type sourceType, params Type[] targetTypes)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (targetTypes == null) throw new ArgumentNullException(nameof(targetTypes));

            return targetTypes.Any(sourceType.IsAssignableTo);
        }

        public static bool IsAssignableTo(this Type sourceType, Type targetType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            if (targetType.IsAssignableFrom(sourceType))
                return true;

            foreach (var baseType in sourceType.GetBaseTypesAndSelf())
            {
                if (targetType.IsAssignableFrom(baseType))
                    return true;

                if (baseType.IsGenericType && targetType.IsGenericTypeDefinition &&
                    baseType.GetGenericTypeDefinition() == targetType)
                    return true;
            }

            return false;
        }

        public static IEnumerable<Type> GetBaseTypesAndSelf(this Type type, bool includeInterfaces = true)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            yield return type;

            foreach (var baseType in type.GetBaseTypes(includeInterfaces))
                yield return baseType;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type, bool includeInterfaces = true)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (includeInterfaces)
            {
                foreach (var implementedInterface in type.GetInterfaces())
                    yield return implementedInterface;
            }

            if (type.BaseType != null)
            {
                yield return type.BaseType;

                foreach (var baseType in type.BaseType.GetBaseTypes())
                    yield return baseType;
            }
        }

        public static IReadOnlyList<Type> GetGenericTypeParameters(this Type genericType,
            Type genericTypeDefinition)
        {
            if (genericType == null) throw new ArgumentNullException(nameof(genericType));
            if (genericTypeDefinition == null) throw new ArgumentNullException(nameof(genericTypeDefinition));

            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                var message = $"{genericTypeDefinition.GetDisplayName()} is not a generic type definition.";
                throw new ArgumentException(message, nameof(genericTypeDefinition));
            }

            var results = new List<Type>();

            foreach (var baseType in genericType.GetBaseTypesAndSelf())
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericTypeDefinition)
                    results.Add(baseType);
            }

            if (results.Count == 0)
            {
                var message =
                    $"{genericType.GetDisplayName()} does not " +
                    $"implement {genericTypeDefinition.GetDisplayName()}.";
                
                throw new ArgumentException(message, nameof(genericType));
            }

            if (results.Count >= 2)
            {
                var message = new StringBuilder();
                message.AppendLine(
                    $"{genericType.GetDisplayName()} implements {genericTypeDefinition.GetDisplayName()} " +
                    $"multiple times:");

                foreach (var result in results)
                    message.AppendLine($"  - {result.GetDisplayName()}");
                
                throw new ArgumentException(message.ToString().Trim(), nameof(genericType));
            }

            return results[0].GetGenericArguments();
        }
    }
}
