using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Runtime
{
    public class ValueEqualityComparerProvider : IValueEqualityComparerProvider
    {
        private readonly IMemberProvider _memberProvider;
        private readonly IMetadataProvider _metadataProvider;
        private readonly ConcurrentDictionary<EqualityComparerScope, IEqualityComparer> _cachedEqualityComparers;

        public ValueEqualityComparerProvider(IMemberProvider memberProvider, IMetadataProvider metadataProvider)
        {
            _memberProvider = memberProvider ?? throw new ArgumentNullException(nameof(memberProvider));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            _cachedEqualityComparers = new ConcurrentDictionary<EqualityComparerScope, IEqualityComparer>();
        }

        public IEqualityComparer GetEqualityComparer(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var scope = EqualityComparerScope.ForType(type);
            return _cachedEqualityComparers.GetOrAdd(scope, s =>
            {
                var metadata = _metadataProvider.GetTypeMetadata(s.TargetType);
                var context = new EqualityComparerContext(s, metadata);

                return CreateEqualityComparer(context);
            });
        }

        private IEqualityComparer GetEqualityComparer(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            
            var scope = EqualityComparerScope.ForMember(member);
            return _cachedEqualityComparers.GetOrAdd(scope, s =>
            {
                var memberMetadata = _metadataProvider.GetMemberMetadata(member);
                if (memberMetadata.IsEmpty)
                    return GetEqualityComparer(member.MemberType);

                var typeMetadata = _metadataProvider.GetTypeMetadata(member.MemberType);
                var effectiveMetadata = typeMetadata.OverrideWith(memberMetadata);

                var context = new EqualityComparerContext(s, effectiveMetadata);
                return CreateEqualityComparer(context);
            });
        }

        private IEqualityComparer CreateEqualityComparer(EqualityComparerContext context)
        {
            return context.Metadata.TryGetMetadata<IEqualityComparerMetadata>()
                .Match(equalityComparerMetadata => equalityComparerMetadata.GetEqualityComparer(context), Create);

            IEqualityComparer Create()
            {
                var typeCategory = context.Scope.TargetType.GetCategory();
                switch (typeCategory)
                {
                    case TypeCategory.Simple:
                        return CreateEqualityComparerForSimpleType(context);

                    case TypeCategory.Complex:
                        return CreateEqualityComparerForComplexType(context);

                    case TypeCategory.Collection:
                        return CreateEqualityComparerForCollection(context);

                    default:
                        var message = $"Unknown category {typeCategory} of type {context.Scope.TargetType.GetDisplayName()}.";
                        throw new ArgumentException(message, nameof(context));
                }
            }
        }

        protected virtual IEqualityComparer CreateEqualityComparerForSimpleType(EqualityComparerContext context)
        {
            return DefaultEqualityComparer.GetForType(context.Scope.TargetType);
        }

        protected virtual IEqualityComparer CreateEqualityComparerForComplexType(EqualityComparerContext context)
        {
            var members = _memberProvider.GetMembers(context.Scope.TargetType).Where(IsMemberIncluded);
            var memberComparisonConfigurations = members.Select(CreateMemberComparisonConfiguration);

            return MemberEqualityComparer.Create(context.Scope.TargetType, memberComparisonConfigurations);

            MemberComparisonConfiguration CreateMemberComparisonConfiguration(Member member)
            {
                var equalityComparerReference =
                    new EqualityComparerReference(() => GetEqualityComparer(member));

                return new MemberComparisonConfiguration(member, equalityComparerReference);
            }
        }

        protected virtual bool IsMemberIncluded(Member member)
        {
            var metadata = _metadataProvider.GetMemberMetadata(member);
            return !metadata.HasMetadata<IIgnoredMemberMetadata>();
        }

        protected virtual IEqualityComparer CreateEqualityComparerForCollection(EqualityComparerContext context)
        {
            var elementType = context.Scope.TargetType.GetCollectionElementType();
            var elementEqualityComparer = GetElementEqualityComparer(context, elementType);

            var category = context.Metadata.TryGetMetadata<ICollectionCategoryMetadata>()
                .Match(m => m.GetCollectionCategory(context), context.Scope.TargetType.GetCollectionCategory);
            
            switch (category)
            {
                case CollectionCategory.Sequence:
                    return SequenceEqualityComparer.Create(elementType, elementEqualityComparer);

                case CollectionCategory.Set:
                    return SetEqualityComparer.Create(elementType, elementEqualityComparer);

                case CollectionCategory.Bag:
                    return BagEqualityComparer.Create(elementType, elementEqualityComparer);

                default:
                    var message = $"Unknown category {category} of collection {context.Scope.TargetType.GetDisplayName()}";
                    throw new ArgumentException(message, nameof(category));
            }
        }

        protected virtual IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context, Type elementType)
        {
            return context.Metadata.TryGetMetadata<IElementEqualityComparerMetadata>()
                .Match(m => m.GetElementEqualityComparer(context), () => GetEqualityComparer(elementType));
        }
    }
}
