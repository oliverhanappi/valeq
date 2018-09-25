using System;
using System.Collections.Generic;
using Valeq.Reflection;

namespace Valeq.Utils
{
    public struct OptionalValue<T>
    {
        public static bool operator ==(OptionalValue<T> left, OptionalValue<T> right) => left.Equals(right);
        public static bool operator !=(OptionalValue<T> left, OptionalValue<T> right) => !left.Equals(right);

        public static implicit operator OptionalValue<T>(T value) => new OptionalValue<T>(value);
        public static readonly OptionalValue<T> None = default(OptionalValue<T>);

        private readonly bool _hasValue;
        private readonly T _value;

        public OptionalValue(T value)
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException(nameof(value));

            _value = value;
            _hasValue = true;
        }

        public TResult Match<TResult>(Func<T, TResult> mapper, Func<TResult> fallback)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));

            var result = _hasValue ? mapper(_value) : fallback();
            if (ReferenceEquals(result, null))
                throw new InvalidOperationException($"Match operation has produced null for {this}.");

            return result;
        }

        //TODO public T IfNone(Func<T> fallback) => Match(x => x, fallback);

        public bool Equals(OptionalValue<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OptionalValue<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_value);
        }

        public override string ToString()
        {
            return _hasValue
                ? $"Some({_value}) of {typeof(T).GetDisplayName()}"
                : $"None of {typeof(T).GetDisplayName()}";
        }
    }
}
