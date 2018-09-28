using System;
using Valeq.Configuration;
using Valeq.Metadata;
using Valeq.Reflection;
using Valeq.Utils;

namespace Valeq.Runtime
{
    public class EqualityComparerContext
    {
        public EqualityComparerScope Scope { get; }
        public MetadataCollection Metadata { get; }
        public IValueEqualityComparerProvider ValueEqualityComparerProvider { get; }
        public ValueEqualityConfiguration Configuration { get; }

        public EqualityComparerContext(EqualityComparerScope scope, MetadataCollection metadata,
            IValueEqualityComparerProvider valueEqualityComparerProvider, ValueEqualityConfiguration configuration)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            ValueEqualityComparerProvider = valueEqualityComparerProvider ??
                                            throw new ArgumentNullException(nameof(valueEqualityComparerProvider));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
