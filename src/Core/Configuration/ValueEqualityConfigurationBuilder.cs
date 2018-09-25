using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Configuration
{
    public class ValueEqualityConfigurationBuilder
    {
        public ValueEqualityConfiguration Build()
        {
            var metadataProvider = BuildMetadataProvider();
            var memberProvider = BuildMemberProvider();
            var valueEqualityComparerProvider = BuildValueEqualityComparerProvider();

            return new ValueEqualityConfiguration(memberProvider, metadataProvider, valueEqualityComparerProvider);

            IMetadataProvider BuildMetadataProvider()
            {
                return new CachingMetadataProviderDecorator(new AttributeMetadataProvider());
            }

            IMemberProvider BuildMemberProvider()
            {
                return new CachingMemberProviderDecorator(new MetadataBasedMemberProvider(metadataProvider));
            }

            IValueEqualityComparerProvider BuildValueEqualityComparerProvider()
            {
                return new ValueEqualityComparerProvider(memberProvider, metadataProvider);
            }
        }
    }
}
