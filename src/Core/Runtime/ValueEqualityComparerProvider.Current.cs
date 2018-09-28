using System;
using Valeq.Configuration;

namespace Valeq.Runtime
{
    public partial class ValueEqualityComparerProvider
    {
        private static IValueEqualityComparerProvider _current;
        private static Func<IValueEqualityComparerProvider> _currentGetter;
        private static Action<IValueEqualityComparerProvider> _currentSetter;

        static ValueEqualityComparerProvider()
        {
            ResetCurrentAccessStrategy();
        }

        public static IValueEqualityComparerProvider Current
        {
            get
            {
                var current = _currentGetter.Invoke();
                if (current == null)
                {
                    current = CreateDefault();
                    _currentSetter.Invoke(current);
                }

                return current;
            }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _currentSetter.Invoke(value);
            }
        }
        
        public static void SetCurrentAccessStrategy(Func<IValueEqualityComparerProvider> getter, Action<IValueEqualityComparerProvider> setter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (setter == null) throw new ArgumentNullException(nameof(setter));

            _currentGetter = getter;
            _currentSetter = setter;
        }

        public static void ResetCurrentAccessStrategy()
        {
            SetCurrentAccessStrategy(() => _current, c => _current = c);
        }

        public static IValueEqualityComparerProvider CreateDefault()
        {
            var defaultConfiguration = ValueEqualityConfiguration.CreateDefaultConfiguration();
            return new ValueEqualityComparerProvider(defaultConfiguration);
        }

        public static void Configure(Action<ValueEqualityConfigurationBuilder> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var builder = new ValueEqualityConfigurationBuilder();
            configure.Invoke(builder);

            var configuration = builder.Build();
            Current = new ValueEqualityComparerProvider(configuration);
        }
    }
}
