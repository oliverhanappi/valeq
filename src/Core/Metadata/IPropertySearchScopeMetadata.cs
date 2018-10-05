using Valeq.Configuration;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [Metadata]
    public interface IPropertySearchScopeMetadata : IMetadata
    {
        PropertySearchScope GetPropertySearchScope(EqualityComparerContext context);
    }
}
