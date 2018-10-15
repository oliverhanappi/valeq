using System;
using Valeq.Configuration;
using Valeq.Metadata;
using Valeq.Runtime;
using Valeq.Utils;

namespace Valeq.TestDataBuilders
{
    public static class EqualityComparerContextBuilder
    {
        public static Builder EqualityComparerContext(this Create _) => new Builder();

        public class Builder : Builder<EqualityComparerContext>
        {
            private OptionalValue<Type> _targetType;
            private OptionalValue<MetadataCollection> _metadata;
            private OptionalValue<ValueEqualityConfiguration> _configuration;

            public Builder ForType<T>() => ForType(typeof(T));
            
            public Builder ForType(Type targetType)
            {
                _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
                return this;
            }

            public Builder WithMetadata(MetadataCollection metadata)
            {
                _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
                return this;
            }

            public Builder WithConfiguration(ValueEqualityConfiguration configuration)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                return this;
            }
            
            public override EqualityComparerContext Build()
            {
                var targetType = _targetType.IfNone(typeof(object));
                var scope = EqualityComparerScope.ForType(targetType);
                var metadata = _metadata.IfNone(MetadataCollection.Empty);
                var configuration = _configuration.IfNone(ValueEqualityConfiguration.CreateDefaultConfiguration);
                
                return new EqualityComparerContext(scope, metadata, configuration);
            }
        }
    }
}
