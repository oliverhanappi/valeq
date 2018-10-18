using System;
using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Configuration
{
    public class ValueEqualityConfiguration
    {
        public static ValueEqualityConfiguration CreateDefaultConfiguration()
        {
            return new ValueEqualityConfigurationBuilder().Build();
        }

        public IMemberProvider MemberProvider { get; }
        public IMetadataProvider MetadataProvider { get; }
        public IActivator Activator { get; }

        public EqualityComparisonType DefaultEqualityComparisonType { get; }
        public StringComparisonCulture DefaultStringComparisonCulture { get; }
        public PropertySearchScope DefaultPropertySearchScope { get; }

        public ValueEqualityConfiguration(
            IMemberProvider memberProvider,
            IMetadataProvider metadataProvider,
            IActivator activator,
            EqualityComparisonType defaultEqualityComparisonType,
            StringComparisonCulture defaultStringComparisonCulture,
            PropertySearchScope defaultPropertySearchScope)
        {
            MemberProvider = memberProvider ?? throw new ArgumentNullException(nameof(memberProvider));
            MetadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            Activator = activator ?? throw new ArgumentNullException(nameof(activator));
            DefaultStringComparisonCulture = defaultStringComparisonCulture;
            DefaultPropertySearchScope = defaultPropertySearchScope;
            DefaultEqualityComparisonType = defaultEqualityComparisonType;
        }
    }
}
