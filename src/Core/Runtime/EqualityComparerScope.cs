using System;
using Valeq.Reflection;
using Valeq.Utils;

namespace Valeq.Runtime
{
    public class EqualityComparerScope
    {
        public static EqualityComparerScope ForType(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            return new EqualityComparerScope(targetType, OptionalValue<Member>.None);
        }

        public static EqualityComparerScope ForMember(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            return new EqualityComparerScope(member.MemberType, member);
        }
        
        public static bool operator ==(EqualityComparerScope left, EqualityComparerScope right) => Equals(left, right);
        public static bool operator !=(EqualityComparerScope left, EqualityComparerScope right) => !Equals(left, right);

        public Type TargetType { get; }
        public OptionalValue<Member> Member { get; }

        private EqualityComparerScope(Type targetType, OptionalValue<Member> member)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            Member = member;
        }

        public EqualityComparerScope GetScopeForUnderlyingTypeOfNullable()
        {
            var underlyingType = Nullable.GetUnderlyingType(TargetType);
            if (underlyingType == null)
                throw new InvalidOperationException($"{this} does not target a nullable type.");
            
            return new EqualityComparerScope(underlyingType, Member);
        }

        protected bool Equals(EqualityComparerScope other)
        {
            return TargetType == other.TargetType && Member.Equals(other.Member);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EqualityComparerScope) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TargetType.GetHashCode() * 397) ^ Member.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Member.Match(m => $"MemberScope for {m}", () => $"TypeScope for {TargetType.GetDisplayName()}");
        }
    }
}
