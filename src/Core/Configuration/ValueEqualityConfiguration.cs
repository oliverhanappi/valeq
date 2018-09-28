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

        public StringComparisonCulture DefaultStringComparisonCulture { get; }

        public ValueEqualityConfiguration(IMemberProvider memberProvider, IMetadataProvider metadataProvider,
            StringComparisonCulture defaultStringComparisonCulture)
        {
            MemberProvider = memberProvider ?? throw new ArgumentNullException(nameof(memberProvider));
            MetadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            DefaultStringComparisonCulture = defaultStringComparisonCulture;
        }
    }
}
