using System.Reflection;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Configuration
{
    public class ValueEqualityConfigurationBuilder
    {
        public ICustomMetadataBuilder Compare { get; }

        public StringComparisonCulture DefaultStringComparisonCulture { get; set; }

        public ValueEqualityConfigurationBuilder()
        {
            DefaultStringComparisonCulture = StringComparisonCulture.None;
            Compare = new CustomMetadataBuilder();
        }

        public ValueEqualityConfiguration Build()
        {
            ApplyDefaultMetadata();

            var metadataProvider = BuildMetadataProvider();
            var memberProvider = BuildMemberProvider();

            return new ValueEqualityConfiguration(memberProvider, metadataProvider, DefaultStringComparisonCulture);

            IMetadataProvider BuildMetadataProvider()
            {
                var attributeMetadataProvider = new AttributeMetadataProvider();
                var customMetadataProvider = Compare.BuildMetadataProvider();

                var aggregateMetadataProvider =
                    new AggregateMetadataProvider(attributeMetadataProvider, customMetadataProvider);
                
                return new CachingMetadataProviderDecorator(aggregateMetadataProvider);
            }

            IMemberProvider BuildMemberProvider()
            {
                return new CachingMemberProviderDecorator(new MetadataBasedMemberProvider(metadataProvider));
            }
        }

        private void ApplyDefaultMetadata()
        {
            var stringComparison = DefaultStringComparisonCulture.ToStringComparison(ignoreCase: false);
            Compare.Type<string>().AddTypeMetadataIfNotExists(new StringComparisonAttribute(stringComparison));
        }
    }
}
