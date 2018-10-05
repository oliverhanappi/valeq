using System.Collections;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [Metadata]
    public interface IElementEqualityComparerMetadata : IMetadata
    {
        IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context);
    }
}
