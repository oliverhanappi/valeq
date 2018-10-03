using System;
using System.Reflection;

namespace Valeq.Reflection
{
    public class Member
    {
        public static bool operator ==(Member x, Member y) => Equals(x, y);
        public static bool operator !=(Member x, Member y) => !Equals(x, y);

        public static Member FromFieldInfo(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            var name = $"Field {fieldInfo.GetDisplayName()}";
            var getter = MemberAccess.CreateFieldGetter(fieldInfo);
            return new Member(name, fieldInfo, fieldInfo.FieldType, getter, fieldInfo.IsPartOf);
        }

        public static Member FromPropertyInfo(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            var name = $"Property {propertyInfo.GetDisplayName()}";
            var getter = MemberAccess.CreatePropertyGetter(propertyInfo);
            return new Member(name, propertyInfo, propertyInfo.PropertyType, getter, propertyInfo.IsPartOf);
        }

        private readonly Func<object, object> _getter;
        private readonly Func<Type, bool> _isPartOf;

        public object MemberSource { get; }
        public string Name { get; }
        public Type MemberType { get; }

        public Member(string name, object memberSource, Type memberType,
            Func<object, object> getter, Func<Type, bool> isPartOf)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            MemberSource = memberSource ?? throw new ArgumentNullException(nameof(memberSource));
            MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));

            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
            _isPartOf = isPartOf ?? throw new ArgumentNullException(nameof(isPartOf));
        }

        public bool IsPartOf(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return _isPartOf.Invoke(type);
        }

        public object GetValue(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (!IsPartOf(instance.GetType()))
                throw new ArgumentException($"{this} is not part of {instance.GetType().GetDisplayName()}");

            try
            {
                return _getter.Invoke(instance);
            }
            catch (Exception ex)
            {
                throw new MemberRetrievalException(this, instance, ex);
            }
        }

        public override string ToString()
        {
            return $"{Name}:{MemberType.GetDisplayName()}";
        }

        protected bool Equals(Member other)
        {
            return MemberSource.Equals(other.MemberSource);
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
            return MemberSource.GetHashCode();
        }
    }
}
