using System.Collections;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [Metadata]
    public interface IEqualityComparerMetadata : IMetadata
    {
        IEqualityComparer GetEqualityComparer(EqualityComparerContext context);
    }
}
