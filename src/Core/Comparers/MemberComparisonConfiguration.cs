using System;
using System.Collections;
using Valeq.Reflection;

namespace Valeq.Comparers
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
