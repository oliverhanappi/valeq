using System;
using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class CustomMemberMetadata : CustomMetadata
    {
        public Member Member { get; }

        public CustomMemberMetadata(Member member, IMetadata metadata) : base(metadata)
        {
            Member = member ?? throw new ArgumentNullException(nameof(member));
        }
    }
}
