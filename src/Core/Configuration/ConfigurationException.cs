using System;
using System.Runtime.Serialization;

namespace Valeq.Configuration
{
    [Serializable]
    public class ConfigurationException : Exception
    {
        protected ConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
