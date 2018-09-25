using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class CachingMetadataProviderDecorator : IMetadataProvider
    {
        private readonly IMetadataProvider _inner;
        private readonly ConcurrentDictionary<Type, MetadataCollection> _cachedTypeMetadata;
        private readonly ConcurrentDictionary<Member, MetadataCollection> _cachedMemberMetadata;

        public CachingMetadataProviderDecorator(IMetadataProvider inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _cachedTypeMetadata = new ConcurrentDictionary<Type, MetadataCollection>();
            _cachedMemberMetadata = new ConcurrentDictionary<Member, MetadataCollection>();
        }

        public MetadataCollection GetTypeMetadata(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return _cachedTypeMetadata.GetOrAdd(type, t => _inner.GetTypeMetadata(t));
        }

        public MetadataCollection GetMemberMetadata(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            return _cachedMemberMetadata.GetOrAdd(member, m => _inner.GetMemberMetadata(m));
        }
    }
}
