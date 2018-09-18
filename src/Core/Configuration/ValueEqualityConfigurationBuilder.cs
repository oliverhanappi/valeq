using Valueq.Comparers;
using Valueq.Reflection;

namespace Valueq.Configuration
{
    public class ValueEqualityConfigurationBuilder
    {
        public ValueEqualityConfiguration Build()
        {
            var memberProvider = BuildMemberProvider();
            var valueEqualityComparerProvider = BuildValueEqualityComparerProvider();

            return new ValueEqualityConfiguration(memberProvider, valueEqualityComparerProvider);

            IMemberProvider BuildMemberProvider()
            {
                return new CachingMemberProviderDecorator(new FieldMemberProvider());
            }

            IValueEqualityComparerProvider BuildValueEqualityComparerProvider()
            {
                return new ValueEqualityComparerProvider(memberProvider);
            }
        }
    }
}
