namespace Domain
{
    public class CustomException : Exception
    {
        public CustomExceptionType ExceptionType { get; protected set; }

        public CustomException() { }
        public CustomException(string message) : base(message) { }
        public CustomException(string message, Exception inner) : base(message, inner) { }
        public CustomException(CustomExceptionType exceptionType) : base("")
        {
            ExceptionType = exceptionType;
        }
        public CustomException(CustomExceptionType exceptionType, string message) : base(message)
        {
            ExceptionType = exceptionType;
        }
        public CustomException(CustomExceptionType exceptionType, string message, Exception inner) : base(message, inner)
        {
            ExceptionType = exceptionType;
        }
    }
}