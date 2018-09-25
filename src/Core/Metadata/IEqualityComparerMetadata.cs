using System;
using System.Collections;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Metadata
{
    [Metadata]
    public interface IEqualityComparerMetadata : IMetadata
    {
        IEqualityComparer GetEqualityComparer(EqualityComparerContext context);
    }

    [Metadata]
    public interface IElementEqualityComparerMetadata : IMetadata
    {
        IEqualityComparer GetElementEqualityComparer(EqualityComparerContext context);
    }

    [Metadata]
    public interface ICollectionCategoryMetadata : IMetadata
    {
        CollectionCategory GetCollectionCategory(EqualityComparerContext context);
    }
}
