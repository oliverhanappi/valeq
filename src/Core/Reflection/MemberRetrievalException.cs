using System;
using System.Runtime.Serialization;

namespace Valeq.Reflection
{
    [Serializable]
    public class MemberRetrievalException : Exception
    {
        public MemberRetrievalException(Member accessedMember, object instance, Exception innerException)
            : base($"An exception occurred while accessing {accessedMember} of {instance}.", innerException)
        {
        }

        protected MemberRetrievalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
