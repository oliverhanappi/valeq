using System;
using System.Collections.Generic;
using System.Linq;
using Valeq.Metadata;

namespace Valeq.Configuration
{
    public class CustomMetadataBuilder : ICustomMetadataBuilder
    {
        private readonly IDictionary<Type, ICustomTypeMetadataBuilder> _typeMetadataBuilders =
            new Dictionary<Type, ICustomTypeMetadataBuilder>();

        public ICustomTypeMetadataBuilder<TType> Type<TType>()
        {
            if (!_typeMetadataBuilders.TryGetValue(typeof(TType), out var typeMetadataBuilder))
            {
                typeMetadataBuilder = new CustomTypeMetadataBuilder<TType>(this);
                _typeMetadataBuilders.Add(typeof(TType), typeMetadataBuilder);
            }

            return (ICustomTypeMetadataBuilder<TType>) typeMetadataBuilder;
        }

        public IMetadataProvider BuildMetadataProvider()
        {
            var customMetadata = _typeMetadataBuilders.Values.SelectMany(b => b.GetCustomMetadata());
            return new CustomMetadataProvider(customMetadata);
        }
    }
}