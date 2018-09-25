using System;
using System.Collections.Generic;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public interface IMetadataProvider
    {
        MetadataCollection GetTypeMetadata(Type type);
        MetadataCollection GetMemberMetadata(Member member);
    }
}
