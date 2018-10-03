using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Valeq.Comparers;
using Valeq.Configuration;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Runtime
{
    public partial class ValueEqualityComparerProvider : IValueEqualityComparerProvider
    {
        public ValueEqualityConfiguration Configuration { get; }

        private readonly ConcurrentDictionary<EqualityComparerScope, IEqualityComparer> _cachedEqualityComparers;

        public ValueEqualityComparerProvider(ValueEqualityConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cachedEqualityComparers = new ConcurrentDictionary<EqualityComparerScope, IEqualityComparer>();
        }

        public IEqualityComparer GetEqualityComparer(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var scope = EqualityComparerScope.ForType(type);
            return _cachedEqualityComparers.GetOrAdd(scope, s =>
            {
                var metadata = Configuration.MetadataProvider.GetTypeMetadata(s.TargetType);
                var context = new EqualityComparerContext(s, metadata, this, Configuration);

                return CreateEqualityComparer(context);
            });
        }

        private IEqualityComparer GetEqualityComparer(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var scope = EqualityComparerScope.ForMember(member);
            return _cachedEqualityComparers.GetOrAdd(scope, s =>
            {
                var memberMetadata = Configuration.MetadataProvider.GetMemberMetadata(member);
                if (memberMetadata.IsEmpty)
                    return GetEqualityComparer(member.MemberType);

                var typeMetadata = Configuration.MetadataProvider.GetTypeMetadata(member.MemberType);
                var effectiveMetadata = typeMetadata.OverrideWith(memberMetadata);

                var context = new EqualityComparerContext(s, effectiveMetadata, this, Configuration);
                return CreateEqualityComparer(context);
            });
        }

        private IEqualityComparer GetEqualityComparer(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return _cachedEqualityComparers.GetOrAdd(context.Scope, s => CreateEqualityComparer(context));
        }

        private IEqualityComparer CreateEqualityComparer(EqualityComparerContext context)
        {
            return context.Metadata.TryGetMetadata<IEqualityComparerMetadata>()
                .Match(equalityComparerMetadata => equalityComparerMetadata.GetEqualityComparer(context), Create);

            IEqualityComparer Create()
            {
                var equalityComparisonType = context.Metadata.TryGetMetadata<IEqualityComparisonTypeMetadata>()
                    .Match(m => m.EqualityComparisonType, () => Configuration.DefaultEqualityComparisonType);

                switch (equalityComparisonType)
                {
                    case EqualityComparisonType.DefaultEquality:
                        return CreateDefaultEqualityComparer(context);

                    case EqualityComparisonType.ValueEquality:
                        return CreateValueEqualityComparer(context);

                    default:
                        throw new ArgumentException($"Unknown equality comparison type: {equalityComparisonType}");
                }
            }
        }

        private IEqualityComparer CreateDefaultEqualityComparer(EqualityComparerContext context)
        {
            return DefaultEqualityComparer.GetForType(context.Scope.TargetType);
        }

        private IEqualityComparer CreateValueEqualityComparer(EqualityComparerContext context)
        {
            var underlyingType = Nullable.GetUnderlyingType(context.Scope.TargetType);
            if (underlyingType != null)
            {
                var underlyingContext = context.GetContextForUnderlyingTypeOfNullable();
                return GetEqualityComparer(underlyingContext);
            }
            
            var typeCategory = context.Scope.TargetType.GetCategory();
            switch (typeCategory)
            {
                case TypeCategory.Simple:
                    return CreateValueEqualityComparerForSimpleType(context);

                case TypeCategory.Complex:
                    return CreateValueEqualityComparerForComplexType(context);

                case TypeCategory.Collection:
                    return CreateValueEqualityComparerForCollection(context);

                default:
                    var message =
                        $"Unknown category {typeCategory} of type {context.Scope.TargetType.GetDisplayName()}.";
                    throw new ArgumentException(message, nameof(context));
            }
        }

        protected virtual IEqualityComparer CreateValueEqualityComparerForSimpleType(EqualityComparerContext context)
        {
            return CreateDefaultEqualityComparer(context);
        }

        protected virtual IEqualityComparer CreateValueEqualityComparerForComplexType(EqualityComparerContext context)
        {
            var members = Configuration.MemberProvider.GetMembers(context.Scope.TargetType).Where(IsMemberIncluded);
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
            var metadata = Configuration.MetadataProvider.GetMemberMetadata(member);
            return !metadata.HasMetadata<IIgnoredMemberMetadata>();
        }

        protected virtual IEqualityComparer CreateValueEqualityComparerForCollection(EqualityComparerContext context)
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
                    var message =
                        $"Unknown category {category} of collection {context.Scope.TargetType.GetDisplayName()}";
                    throw new ArgumentException(message, nameof(category));
            }
        }

        protected virtual IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context,
            Type elementType)
        {
            return context.Metadata.TryGetMetadata<IElementEqualityComparerMetadata>()
                .Match(m => m.GetElementEqualityComparer(context), () => GetEqualityComparer(elementType));
        }
    }
}
