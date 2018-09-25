using System;
using System.Reflection;

namespace Valeq.Reflection
{
    public class Member
    {
        public static Member FromFieldInfo(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            var fieldGetter = MemberAccess.CreateFieldGetter(fieldInfo);
            return new Member(fieldInfo, fieldInfo.FieldType, fieldGetter);
        }

        public static Member FromPropertyInfo(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            var propertyGetter = MemberAccess.CreatePropertyGetter(propertyInfo);
            return new Member(propertyInfo, propertyInfo.PropertyType, propertyGetter);
        }

        private readonly Func<object, object> _getter;

        public MemberInfo MemberInfo { get; }
        public string Name { get; }
        public Type MemberType { get; }

        private Member(MemberInfo memberInfo, Type memberType, Func<object, object> getter)
        {
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));

            Name = memberInfo.GetDisplayName();
            MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));
        }

        public bool IsPartOf(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (MemberInfo.DeclaringType == type)
                return true;

            if (type.BaseType == null)
                return false;

            return IsPartOf(type.BaseType);
        }

        public object GetValue(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            return _getter.Invoke(instance);
        }

        public override string ToString()
        {
            var type = MemberInfo is FieldInfo ? "Field" : "Property";
            return $"{type} {Name}:{MemberType.GetDisplayName()} of {MemberInfo.DeclaringType.GetDisplayName()}";
        }

        protected bool Equals(Member other)
        {
            return MemberInfo.Equals(other.MemberInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Member) obj);
        }

        public override int GetHashCode()
        {
            return MemberInfo.GetHashCode();
        }
    }
}
