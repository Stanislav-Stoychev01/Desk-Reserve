using System.Runtime.Serialization;

namespace DeskReserve.Exceptions
{
    [Serializable]
    public class DataNotFound : Exception
    {
        public DataNotFound()
        { }

        public DataNotFound(string message)
            : base(message)
        { }

        public DataNotFound(string message, Exception inner)
            : base(message, inner)
        { }
    }
}