using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Valueq.Reflection;

namespace Valueq.Comparers
{
    public static class MemberEqualityComparer
    {
        public static IEqualityComparer Create(Type type, IEnumerable<MemberComparisonConfiguration> members)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (members == null) throw new ArgumentNullException(nameof(members));

            var genericMemberEqualityComparerType = typeof(MemberEqualityComparer<>).MakeGenericType(type);
            return (IEqualityComparer) Activator.CreateInstance(genericMemberEqualityComparerType, members);
        }
    }
    
    public class MemberEqualityComparer<T> : GenericEqualityComparer<T>
    {
        private readonly IReadOnlyList<MemberComparisonConfiguration> _members;

        public MemberEqualityComparer(IEnumerable<MemberComparisonConfiguration> members)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            _members = members.ToList();
            
            foreach (var member in _members)
            {
                if (!member.Member.IsPartOf(typeof(T)))
                    throw new ArgumentException($"{member} is not part of {typeof(T).GetDisplayName()}.", nameof(members));
            }
        }

        protected override bool EqualsInternal(T x, T y)
        {
            foreach (var member in _members)
            {
                var memberValueX = member.Member.GetValue(x);
                var memberValueY = member.Member.GetValue(y);

                if (!member.EqualityComparer.Equals(memberValueX, memberValueY))
                    return false;
            }

            return true;
        }

        protected override int GetHashCodeInternal(T obj)
        {
            unchecked
            {
                var hashCode = 0;

                foreach (var member in _members)
                {
                    var memberValue = member.Member.GetValue(obj);
                    hashCode = (hashCode * 397) ^ member.EqualityComparer.GetHashCode(memberValue);
                }

                return hashCode;
            }
        }
    }
}
