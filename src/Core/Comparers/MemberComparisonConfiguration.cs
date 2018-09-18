using System;
using System.Collections;
using Valueq.Reflection;

namespace Valueq.Comparers
{
    public class MemberComparisonConfiguration
    {
        public Member Member { get; }
        public IEqualityComparer EqualityComparer { get; }

        public MemberComparisonConfiguration(Member member, IEqualityComparer equalityComparer)
        {
            Member = member ?? throw new ArgumentNullException(nameof(member));
            EqualityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
        }
    }
}