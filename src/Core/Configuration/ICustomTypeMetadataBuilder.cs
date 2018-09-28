using System.Collections.Generic;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Configuration
{
    public interface ICustomTypeMetadataBuilder
    {
        IEnumerable<CustomMetadata> GetCustomMetadata();
    }

    public interface ICustomTypeMetadataBuilder<TType> : ICustomTypeMetadataBuilder
    {
        void AddTypeMetadata(IMetadata metadata);
        bool AddTypeMetadataIfNotExists(IMetadata metadata);
        ICustomMemberMetadataBuilder<TType, TMember> GetMemberMetadataBuilder<TMember>(Member member);

        ICustomTypeMetadataBuilder<TOtherType> Type<TOtherType>();
    }
}
