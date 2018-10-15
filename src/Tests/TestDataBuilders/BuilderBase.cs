using System;

namespace Valeq.TestDataBuilders
{
    public abstract class Builder<T>
    {
        public static implicit operator T(Builder<T> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.Build();
        }

        public abstract T Build();
    }
}
