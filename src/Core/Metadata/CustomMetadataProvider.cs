using System;
using System.Collections.Generic;
using System.Linq;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class CustomMetadataProvider : IMetadataProvider
    {
        private readonly IReadOnlyCollection<CustomMetadata> _metadata;

        public CustomMetadataProvider(IEnumerable<CustomMetadata> metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            _metadata = metadata.ToList();
        }

        public MetadataCollection GetTypeMetadata(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var metadata = _metadata
                .OfType<CustomTypeMetadata>()
                .Where(m => m.AppliesTo(type))
                .Select(m => m.Metadata);
            
            return MetadataCollection.ForMetadata(metadata);
        }

        public MetadataCollection GetMemberMetadata(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var metadata = _metadata
                .OfType<CustomMemberMetadata>()
                .Where(m => m.Member == member)
                .Select(m => m.Metadata);

            return MetadataCollection.ForMetadata(metadata);
        }
    }
}
