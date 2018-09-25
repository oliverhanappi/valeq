using System;
using Valeq.Configuration;

namespace Valeq.Metadata
{
    /// <summary>
    /// Marker attribute for actual metadata types which are used and queried for within the runtime.
    /// </summary>
    [AttributeUsage(ValeqAttributeTargets.Type, Inherited = false)]
    public class MetadataAttribute : Attribute
    {
    }
}
