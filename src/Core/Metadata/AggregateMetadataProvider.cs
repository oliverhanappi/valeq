using System;
using System.Collections.Generic;
using System.Linq;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class AggregateMetadataProvider : IMetadataProvider
    {
        private readonly IReadOnlyCollection<IMetadataProvider> _metadataProviders;

        public AggregateMetadataProvider(params IMetadataProvider[] metadataProviders)
            : this(metadataProviders.AsEnumerable())
        {
        }

        public AggregateMetadataProvider(IEnumerable<IMetadataProvider> metadataProviders)
        {
            if (metadataProviders == null)
                throw new ArgumentNullException(nameof(metadataProviders));
            
            _metadataProviders = metadataProviders.ToList();
        }
        
        public MetadataCollection GetTypeMetadata(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var metadatas = _metadataProviders.Select(p => p.GetTypeMetadata(type));
            return MetadataCollection.Merge(metadatas);
        }

        public MetadataCollection GetMemberMetadata(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var metadatas = _metadataProviders.Select(p => p.GetMemberMetadata(member));
            return MetadataCollection.Merge(metadatas);
        }
    }
}
