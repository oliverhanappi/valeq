using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public class CachingMemberProviderDecorator : IMemberProvider
    {
        private readonly IMemberProvider _inner;
        private readonly ConcurrentDictionary<EqualityComparerScope, IReadOnlyCollection<Member>> _cachedMembers;

        public CachingMemberProviderDecorator(IMemberProvider inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _cachedMembers = new ConcurrentDictionary<EqualityComparerScope, IReadOnlyCollection<Member>>();
        }
        
        public IEnumerable<Member> GetMembers(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return _cachedMembers.GetOrAdd(context.Scope, t => _inner.GetMembers(context).ToList());
        }
    }
}
