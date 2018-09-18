using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Valeq.Reflection;

namespace Valeq.Comparers
{
    public class ValueEqualityComparerProvider : IValueEqualityComparerProvider
    {
        private readonly IMemberProvider _memberProvider;
        private readonly ConcurrentDictionary<Type, IEqualityComparer> _cachedEqualityComparers;

        public ValueEqualityComparerProvider(IMemberProvider memberProvider)
        {
            _memberProvider = memberProvider ?? throw new ArgumentNullException(nameof(memberProvider));
            _cachedEqualityComparers = new ConcurrentDictionary<Type, IEqualityComparer>();
        }

        public IEqualityComparer GetEqualityComparer(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return _cachedEqualityComparers.GetOrAdd(type, CreateEqualityComparer);
        }

        private IEqualityComparer CreateEqualityComparer(Type type)
        {
            var typeCategory = type.GetCategory();
            switch (typeCategory)
            {
                case TypeCategory.Simple:
                    return DefaultEqualityComparer.GetForType(type);

                case TypeCategory.Complex:
                    var members = _memberProvider.GetMembers(type);
                    var memberComparisonConfigurations = members.Select(CreateMemberComparisonConfiguration);

                    return MemberEqualityComparer.Create(type, memberComparisonConfigurations);

                case TypeCategory.Collection:
                    return null;

                default:
                    throw new ArgumentException($"Unknown category {typeCategory} of type {type.GetDisplayName()}.", nameof(type));
            }

            MemberComparisonConfiguration CreateMemberComparisonConfiguration(Member member)
            {
                var equalityComparerReference = new EqualityComparerReference(() => GetEqualityComparer(member.MemberType));
                return new MemberComparisonConfiguration(member, equalityComparerReference);
            }
        }
    }
}
