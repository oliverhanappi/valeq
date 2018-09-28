using Valeq.Metadata;

namespace Valeq.Configuration
{
    public interface ICustomMetadataBuilder
    {
        ICustomTypeMetadataBuilder<TType> Type<TType>();

        IMetadataProvider BuildMetadataProvider();
    }
}
