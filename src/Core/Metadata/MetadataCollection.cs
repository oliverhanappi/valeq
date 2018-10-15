using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valeq.Comparers;
using Valeq.Reflection;
using Valeq.Utils;

namespace Valeq.Metadata
{
    public class MetadataCollection
    {
        public static MetadataCollection Empty { get; } =
            new MetadataCollection(new Dictionary<Type, IMetadata>(capacity: 0));

        public static MetadataCollection ForMetadata(IEnumerable<IMetadata> metadata)
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));

            var metadataMap = BuildMetadataMap(metadata);
            return new MetadataCollection(metadataMap);
        }

        public static MetadataCollection Merge(params MetadataCollection[] metadatas)
        {
            if (metadatas == null) throw new ArgumentNullException(nameof(metadatas));
            return Merge(metadatas.AsEnumerable());
        }

        public static MetadataCollection Merge(IEnumerable<MetadataCollection> metadatas)
        {
            if (metadatas == null) throw new ArgumentNullException(nameof(metadatas));

            var metadata = metadatas.SelectMany(m => m._metadata.Values)
                .Distinct(new ReferenceEqualityComparer<IMetadata>());

            var metadataMap = BuildMetadataMap(metadata);
            return new MetadataCollection(metadataMap);
        }

        private static IReadOnlyDictionary<Type, IMetadata> BuildMetadataMap(IEnumerable<IMetadata> metadata)
        {
            var metadataByType = metadata
                .SelectMany(m => m.GetMetadataTypes().Select(t => new {MetadataType = t, Metadata = m}))
                .ToLookup(m => m.MetadataType, m => m.Metadata);

            var duplicateMetadatas = metadataByType.Where(g => g.Count() >= 2).ToList();
            if (duplicateMetadatas.Count > 0)
            {
                var message = new StringBuilder();
                message.AppendLine("The following metadata types are provided multiple times:");
                foreach (var duplicateMetadata in duplicateMetadatas)
                {
                    message.AppendLine($"  - {duplicateMetadata.Key.GetDisplayName()}");
                    foreach (var item in duplicateMetadata)
                        message.AppendLine($"     - {item.GetType().GetDisplayName()}");
                }

                throw new ArgumentException(message.ToString().Trim(), nameof(metadata));
            }

            return metadataByType.ToDictionary(g => g.Key, g => g.Single());
        }

        private readonly IReadOnlyDictionary<Type, IMetadata> _metadata;

        public int Count => _metadata.Count;
        public bool IsEmpty => _metadata.Count == 0;

        private MetadataCollection(IReadOnlyDictionary<Type, IMetadata> metadata)
        {
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public bool HasMetadata<TConcreteMetadata>()
            where TConcreteMetadata : IMetadata
        {
            if (!typeof(TConcreteMetadata).IsMetadataType())
                throw new ArgumentException($"{typeof(TConcreteMetadata).GetDisplayName()} is not a metadata type.");

            return _metadata.ContainsKey(typeof(TConcreteMetadata));
        }

        public OptionalValue<TConcreteMetadata> TryGetMetadata<TConcreteMetadata>()
            where TConcreteMetadata : IMetadata
        {
            if (!typeof(TConcreteMetadata).IsMetadataType())
                throw new ArgumentException($"{typeof(TConcreteMetadata).GetDisplayName()} is not a metadata type.");

            return _metadata.TryGetValue(typeof(TConcreteMetadata), out var metadata)
                ? new OptionalValue<TConcreteMetadata>((TConcreteMetadata) metadata)
                : OptionalValue<TConcreteMetadata>.None;
        }

        public MetadataCollection OverrideWith(MetadataCollection overrides)
        {
            if (overrides == null) throw new ArgumentNullException(nameof(overrides));

            var overriddenMetadata = _metadata.ToDictionary(p => p.Key, p => p.Value);
            foreach (var pair in overrides._metadata)
                overriddenMetadata[pair.Key] = pair.Value;

            return new MetadataCollection(overriddenMetadata);
        }
    }
}
