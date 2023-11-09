namespace DeskReserve.Exceptions
{
    [Serializable]
    public class DataNotFound : Exception
    {
        public string StudentName { get; }

        public DataNotFound() { }

        public DataNotFound(string message)
            : base(message) { }

        public DataNotFound(string message, Exception inner)
            : base(message, inner) { }

        public DataNotFound(string message, string studentName)
            : this(message)
        {
            StudentName = studentName;
        }
    }
}