using System;
using System.Collections.Generic;
using System.Linq;
using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Configuration
{
    public class CustomMemberMetadataBuilder<TType, TMember> : ICustomMemberMetadataBuilder<TType, TMember>
    {
        private readonly Member _member;
        private readonly ICustomTypeMetadataBuilder<TType> _typeMetadataBuilder;
        private readonly IDictionary<Type, IMetadata> _metadataByType = new Dictionary<Type, IMetadata>();

        public CustomMemberMetadataBuilder(Member member, ICustomTypeMetadataBuilder<TType> typeMetadataBuilder)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _typeMetadataBuilder = typeMetadataBuilder ?? throw new ArgumentNullException(nameof(typeMetadataBuilder));
        }
        
        public void AddMemberMetadata(IMetadata metadata)
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            if (ContainsMetadata(metadata))
                throw new ArgumentException($"Cannot add {metadata} because the same type has already been added.");

            foreach (var metadataType in metadata.GetMetadataTypes())
                _metadataByType.Add(metadataType, metadata);
        }

        public bool AddMemberMetadataIfNotExists(IMetadata metadata)
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

        public ICustomTypeMetadataBuilder<TOtherType> Type<TOtherType>()
        {
            return _typeMetadataBuilder.Type<TOtherType>();
        }

        public IEnumerable<CustomMetadata> GetCustomMetadata()
        {
            return _metadataByType.Values
                .Distinct(new ReferenceEqualityComparer<IMetadata>())
                .Select(m => new CustomMemberMetadata(_member, m));
        }
    }
}