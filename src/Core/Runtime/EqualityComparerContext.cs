using System;
using Valeq.Metadata;
using Valeq.Reflection;
using Valeq.Utils;

namespace Valeq.Runtime
{
    public class EqualityComparerContext
    {
        public EqualityComparerScope Scope { get; }
        public MetadataCollection Metadata { get; }

        public EqualityComparerContext(EqualityComparerScope scope, MetadataCollection metadata)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }
    }
}
