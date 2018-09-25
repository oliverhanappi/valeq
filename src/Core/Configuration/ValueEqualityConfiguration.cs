using System;
using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;
using Valeq.Runtime;

namespace Valeq.Configuration
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
        public IMetadataProvider MetadataProvider { get; }
        public IValueEqualityComparerProvider ValueEqualityComparerProvider { get; }

        public ValueEqualityConfiguration(IMemberProvider memberProvider, IMetadataProvider metadataProvider,
            IValueEqualityComparerProvider valueEqualityComparerProvider)
        {
            MemberProvider = memberProvider ?? throw new ArgumentNullException(nameof(memberProvider));
            MetadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            ValueEqualityComparerProvider = valueEqualityComparerProvider ?? throw new ArgumentNullException(nameof(valueEqualityComparerProvider));
        }
    }
}
