using System.Collections.Generic;
using Valeq.Metadata;

namespace Valeq.Configuration
{
    public interface ICustomMemberMetadataBuilder
    {
        IEnumerable<CustomMetadata> GetCustomMetadata();
    }
    
    public interface ICustomMemberMetadataBuilder<TType, TMember> : ICustomMemberMetadataBuilder
    {
        void AddMemberMetadata(IMetadata metadata);
        bool AddMemberMetadataIfNotExists(IMetadata metadata);

        ICustomTypeMetadataBuilder<TOtherType> Type<TOtherType>();
    }
}
