using Valeq.Configuration;

namespace Valeq.Metadata
{
    [Metadata]
    public interface IEqualityComparisonTypeMetadata : IMetadata
    {
        EqualityComparisonType EqualityComparisonType { get; }
    }
}
