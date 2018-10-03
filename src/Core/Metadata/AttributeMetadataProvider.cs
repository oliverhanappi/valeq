using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class AttributeMetadataProvider : IMetadataProvider
    {
        public MetadataCollection GetTypeMetadata(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var metadata = type.GetCustomAttributes().OfType<IMetadata>();
            return MetadataCollection.ForMetadata(metadata);
        }

        public MetadataCollection GetMemberMetadata(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            if (member.MemberSource is MemberInfo memberInfo)
            {
                var metadata = memberInfo.GetCustomAttributes().OfType<IMetadata>();
                return MetadataCollection.ForMetadata(metadata);
            }
            
            return MetadataCollection.Empty;
        }
    }
}
