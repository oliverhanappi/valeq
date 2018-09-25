using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public static class MetadataTypeExtensions
    {
        public static bool IsMetadataType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.GetCustomAttribute<MetadataAttribute>() != null;
        }

        public static IEnumerable<Type> GetMetadataTypes(this IMetadata metadata)
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            return metadata.GetType().GetBaseTypesAndSelf().Where(t => t.IsMetadataType());
        }
    }
}
