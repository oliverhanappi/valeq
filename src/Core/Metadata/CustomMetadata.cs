using System;

namespace Valeq.Metadata
{
    public abstract class CustomMetadata
    {
        public IMetadata Metadata { get; }

        protected CustomMetadata(IMetadata metadata)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }
    }
}