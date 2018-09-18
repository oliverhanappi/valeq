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

        private readonly MemberInfo _memberInfo;
        private readonly Func<object, object> _getter;

        public string Name { get; }
        public Type MemberType { get; }

        private Member(MemberInfo memberInfo, Type memberType, Func<object, object> getter)
        {
            _memberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));

            Name = memberInfo.GetDisplayName();
            MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));
        }

        public bool IsPartOf(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (_memberInfo.DeclaringType == type)
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
            return $"Member {Name}";
        }
    }
}
