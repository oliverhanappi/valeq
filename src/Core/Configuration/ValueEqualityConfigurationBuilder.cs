using Valeq.Comparers;
using Valeq.Reflection;

namespace Valeq.Configuration
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
