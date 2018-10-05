using System;
using Valeq.Configuration;
using Valeq.Metadata;

namespace Valeq.Runtime
{
    public class EqualityComparerContext
    {
        public EqualityComparerScope Scope { get; }
        public MetadataCollection Metadata { get; }
        public ValueEqualityConfiguration Configuration { get; }

        public EqualityComparerContext(EqualityComparerScope scope, MetadataCollection metadata,
            ValueEqualityConfiguration configuration)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public EqualityComparerContext GetContextForUnderlyingTypeOfNullable()
        {
            return new EqualityComparerContext(Scope.GetScopeForUnderlyingTypeOfNullable(), Metadata, Configuration);
        }
    }
}
