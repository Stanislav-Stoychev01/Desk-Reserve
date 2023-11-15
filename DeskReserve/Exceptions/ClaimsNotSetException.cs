using System.Runtime.Serialization;

namespace DeskReserve.Exceptions
{
    [Serializable]
    internal class ClaimsNotSetException : Exception
    {
        public ClaimsNotSetException()
        {
        }

        public ClaimsNotSetException(string message) : base(message)
        {
        }

        public ClaimsNotSetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClaimsNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
