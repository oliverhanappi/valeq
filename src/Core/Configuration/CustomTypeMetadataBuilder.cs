using System;
using System.Collections.Generic;
using System.Linq;
using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Configuration
{
    public class CustomTypeMetadataBuilder<TType> : ICustomTypeMetadataBuilder<TType>
    {
        private readonly ICustomMetadataBuilder _metadataBuilder;
        private readonly IDictionary<Type, IMetadata> _metadataByType = new Dictionary<Type, IMetadata>();

        private readonly IDictionary<Member, ICustomMemberMetadataBuilder> _memberMetadataBuilders =
            new Dictionary<Member, ICustomMemberMetadataBuilder>();

        public CustomTypeMetadataBuilder(ICustomMetadataBuilder metadataBuilder)
        {
            _metadataBuilder = metadataBuilder ?? throw new ArgumentNullException(nameof(metadataBuilder));
        }

        public void AddTypeMetadata(IMetadata metadata)
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            if (ContainsMetadata(metadata))
                throw new ArgumentException($"Cannot add {metadata} because the same type has already been added.");

            foreach (var metadataType in metadata.GetMetadataTypes())
                _metadataByType.Add(metadataType, metadata);
        }

        public bool AddTypeMetadataIfNotExists(IMetadata metadata)
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            if (ContainsMetadata(metadata))
                return false;

            foreach (var metadataType in metadata.GetMetadataTypes())
                _metadataByType.Add(metadataType, metadata);

            return true;
        }

        private bool ContainsMetadata(IMetadata metadata)
        {
            return metadata.GetMetadataTypes().Any(_metadataByType.ContainsKey);
        }

        public ICustomMemberMetadataBuilder<TType, TMember> GetMemberMetadataBuilder<TMember>(Member member)
        {
            if (typeof(TType).GetCategory() != TypeCategory.Complex)
            {
                throw new NotSupportedException("Cannot add member metadata because " +
                                                $"{typeof(TType).GetDisplayName()} is not a complex type.");
            }

            if (member == null) throw new ArgumentNullException(nameof(member));
            if (!member.IsPartOf(typeof(TType)))
            {
                var message = $"{member} is not part of {typeof(TType).GetDisplayName()}.";
                throw new ArgumentException(message, nameof(member));
            }

            if (!_memberMetadataBuilders.TryGetValue(member, out var memberMetadataBuilder))
            {
                memberMetadataBuilder = new CustomMemberMetadataBuilder<TType, TMember>(member, this);
                _memberMetadataBuilders.Add(member, memberMetadataBuilder);
            }

            return (ICustomMemberMetadataBuilder<TType, TMember>) memberMetadataBuilder;
        }

        public ICustomTypeMetadataBuilder<TOtherType> Type<TOtherType>()
        {
            return _metadataBuilder.Type<TOtherType>();
        }

        public IEnumerable<CustomMetadata> GetCustomMetadata()
        {
            var typeMetadata = _metadataByType.Values
                .Distinct(new ReferenceEqualityComparer<IMetadata>())
                .Select(m => new CustomTypeMetadata(typeof(TType), m, inherit: false)); //TODO inherit

            var memberMetadata = _memberMetadataBuilders.Values.SelectMany(m => m.GetCustomMetadata());

            return typeMetadata.Union(memberMetadata);
        }
    }
}
