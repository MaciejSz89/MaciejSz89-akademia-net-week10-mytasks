namespace MyTasks.Persistence.Exceptions
{
    public class ReferencedToAnotherObjectException : Exception
    {
        public ReferencedToAnotherObjectException() : base() { }

        public ReferencedToAnotherObjectException(string message) :base(message){ }
        public ReferencedToAnotherObjectException(string message, Exception exception) : base(message, innerException: exception) { }
    }
}
