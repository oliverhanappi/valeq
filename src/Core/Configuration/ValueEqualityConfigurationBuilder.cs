using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Configuration
{
    public class ValueEqualityConfigurationBuilder
    {
        public ICustomMetadataBuilder Compare { get; }

        public EqualityComparisonType DefaultEqualityComparisonType { get; set; }
        public StringComparisonCulture DefaultStringComparisonCulture { get; set; }
        public PropertySearchScope DefaultPropertySearchScope { get; set; }

        public IActivator Activator { get; set; }

        public ValueEqualityConfigurationBuilder()
        {
            DefaultEqualityComparisonType = EqualityComparisonType.ValueEquality;
            DefaultStringComparisonCulture = StringComparisonCulture.None;
            DefaultPropertySearchScope = PropertySearchScope.OnlyPublic;
            Compare = new CustomMetadataBuilder();
        }

        public ValueEqualityConfiguration Build()
        {
            ApplyDefaultMetadata();

            var metadataProvider = BuildMetadataProvider();
            var memberProvider = BuildMemberProvider();
            var activator = Activator ?? new DefaultActivator();

            return new ValueEqualityConfiguration(memberProvider, metadataProvider, activator,
                DefaultEqualityComparisonType, DefaultStringComparisonCulture, DefaultPropertySearchScope);

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
                return new CachingMemberProviderDecorator(new DispatchingMemberProvider(metadataProvider));
            }
        }

        private void ApplyDefaultMetadata()
        {
            var stringComparison = DefaultStringComparisonCulture.ToStringComparison(ignoreCase: false);
            Compare.Type<string>().AddTypeMetadataIfNotExists(new StringComparisonAttribute(stringComparison));
        }
    }
}
