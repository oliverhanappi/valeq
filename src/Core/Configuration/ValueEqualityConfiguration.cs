using System;
using Valueq.Comparers;
using Valueq.Reflection;

namespace Valueq.Configuration
{
    public class ValueEqualityConfiguration
    {
        private static ValueEqualityConfiguration _currentConfiguration;
        private static Func<ValueEqualityConfiguration> _currentGetter = () => _currentConfiguration;
        private static Action<ValueEqualityConfiguration> _currentSetter = configuration => _currentConfiguration = configuration;

        public static ValueEqualityConfiguration Current
        {
            get
            {
                var currentConfiguration = _currentGetter.Invoke();
                if (currentConfiguration == null)
                {
                    currentConfiguration = CreateDefaultConfiguration();
                    _currentSetter.Invoke(currentConfiguration);
                }

                return currentConfiguration;
            }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _currentSetter.Invoke(value);
            }
        }

        public static ValueEqualityConfiguration CreateDefaultConfiguration()
        {
            return new ValueEqualityConfigurationBuilder().Build();
        }

        public static void SetCurrentAccessMethods(Func<ValueEqualityConfiguration> getter, Action<ValueEqualityConfiguration> setter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (setter == null) throw new ArgumentNullException(nameof(setter));

            _currentGetter = getter;
            _currentSetter = setter;
        }
        
        public IMemberProvider MemberProvider { get; }
        public IValueEqualityComparerProvider ValueEqualityComparerProvider { get; }

        public ValueEqualityConfiguration(IMemberProvider memberProvider, IValueEqualityComparerProvider valueEqualityComparerProvider)
        {
            MemberProvider = memberProvider ?? throw new ArgumentNullException(nameof(memberProvider));
            ValueEqualityComparerProvider = valueEqualityComparerProvider ?? throw new ArgumentNullException(nameof(valueEqualityComparerProvider));
        }
    }
}
