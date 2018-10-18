using Valeq.Configuration;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [Metadata]
    public interface IEqualityComparisonTypeMetadata : IMetadata
    {
        EqualityComparisonType GetEqualityComparisonType(EqualityComparerContext context);
    }
}
