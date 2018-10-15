using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valeq.Metadata;
using Valeq.Runtime;

namespace Valeq.Reflection
{
    public class DispatchingMemberProvider : IMemberProvider
    {
        private readonly IMetadataProvider _metadataProvider;
        private readonly FieldMemberProvider _fieldMemberProvider;
        private readonly PropertyMemberProvider _propertyMemberProvider;

        public DispatchingMemberProvider(IMetadataProvider metadataProvider)
        {
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            _fieldMemberProvider = new FieldMemberProvider();
            _propertyMemberProvider = new PropertyMemberProvider();
        }

        public IEnumerable<Member> GetMembers(EqualityComparerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Scope.TargetType.IsInterface)
                return _propertyMemberProvider.GetMembers(context);

            var fieldMembers = _fieldMemberProvider.GetMembers(context).ToList();
            var fieldMembersWithMetadata =
                fieldMembers.Where(m => !_metadataProvider.GetMemberMetadata(m).IsEmpty).ToList();

            var propertyMembers = _propertyMemberProvider.GetMembers(context).ToList();
            var propertyMembersWithMetadata =
                propertyMembers.Where(m => !_metadataProvider.GetMemberMetadata(m).IsEmpty).ToList();

            if (fieldMembersWithMetadata.Count > 0 && propertyMembersWithMetadata.Count > 0)
            {
                var message = new StringBuilder();
                message.AppendLine($"Unable to choose between comparing fields or properties for " +
                                   $"{context.Scope} because metadata was found on both types of members.");

                message.AppendLine();
                message.AppendLine("Fields with metadata:");
                foreach (var fieldMember in fieldMembersWithMetadata)
                    message.AppendLine($"  - {fieldMember}");

                message.AppendLine();
                message.AppendLine("Properties with metadata:");
                foreach (var propertyMember in propertyMembersWithMetadata)
                    message.AppendLine($"  - {propertyMember}");

                throw new ArgumentException(message.ToString().Trim(), nameof(context));
            }

            return propertyMembersWithMetadata.Count > 0 ? propertyMembers : fieldMembers;
        }
    }
}
