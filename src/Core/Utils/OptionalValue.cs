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

        public bool HasValue { get; }

        private readonly T _value;

        public OptionalValue(T value)
        {
            if (ReferenceEquals(value, null))
            {
                _value = default(T);
                HasValue = false;
            }
            else
            {
                _value = value;
                HasValue = true;
            }
        }

        public TResult Match<TResult>(Func<T, TResult> mapper, Func<TResult> fallback)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));

            var result = HasValue ? mapper.Invoke(_value) : fallback.Invoke();
            if (ReferenceEquals(result, null))
                throw new InvalidOperationException($"Match operation has produced null for {this}.");

            return result;
        }
        
        public bool Equals(OptionalValue<T> other)
        {
            return HasValue == other.HasValue && EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OptionalValue<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (HasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(_value);
            }
        }

        public override string ToString()
        {
            return HasValue
                ? $"Some({_value}) of {typeof(T).GetDisplayName()}"
                : $"None of {typeof(T).GetDisplayName()}";
        }
    }
}
