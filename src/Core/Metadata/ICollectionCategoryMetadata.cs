using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [Metadata]
    public interface ICollectionCategoryMetadata : IMetadata
    {
        CollectionCategory GetCollectionCategory(EqualityComparerContext context);
    }
}
