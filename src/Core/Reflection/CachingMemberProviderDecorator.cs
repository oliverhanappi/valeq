using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Valueq.Reflection
{
    public class CachingMemberProviderDecorator : IMemberProvider
    {
        private readonly IMemberProvider _inner;
        private readonly ConcurrentDictionary<Type, IReadOnlyCollection<Member>> _cachedMembers;

        public CachingMemberProviderDecorator(IMemberProvider inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _cachedMembers = new ConcurrentDictionary<Type, IReadOnlyCollection<Member>>();
        }
        
        public IEnumerable<Member> GetMembers(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return _cachedMembers.GetOrAdd(type, t => _inner.GetMembers(type).ToList());
        }
    }
}