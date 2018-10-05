using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valeq.Configuration;
using Valeq.Metadata;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public class PropertyMemberProvider : IMemberProvider
    {
        public IEnumerable<Member> GetMembers(EqualityComparerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var propertySearchScope = context.Metadata.TryGetMetadata<IPropertySearchScopeMetadata>()
                .Match(m => m.GetPropertySearchScope(context), () => context.Configuration.DefaultPropertySearchScope);

            return context.Scope.TargetType.GetBaseTypesAndSelf(includeInterfaces: false)
                .SelectMany(t => t.GetProperties(GetBindingFlags()))
                .GroupBy(p => p.GetRootPropertyInfo())
                .Select(g => g.OrderByDescending(p => p.DeclaringType, new TypeComparator()).First())
                .Where(p => p.GetCustomAttribute<UndiscoverableMemberAttribute>() == null)
                .Select(Member.FromPropertyInfo);
            
            BindingFlags GetBindingFlags()
            {
                const BindingFlags commonBindingFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly;
                switch (propertySearchScope)
                {
                    case PropertySearchScope.OnlyPublic:
                        return commonBindingFlags | BindingFlags.Public;

                    case PropertySearchScope.All:
                        return commonBindingFlags | BindingFlags.Public | BindingFlags.NonPublic;

                    default:
                        throw new ArgumentException($"Unknown property search scope: {propertySearchScope}");
                }
            }
        }
    }
}
