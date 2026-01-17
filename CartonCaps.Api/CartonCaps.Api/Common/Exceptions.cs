namespace CartonCaps.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
    public class ConflictException : Exception
    {
        public ConflictException(string message)
            : base(message)
        {
        }
    }
    public class UnauthorizedActionException : Exception
    {
        public UnauthorizedActionException(string message)
            : base(message)
        {
        }
    }
    public class ValidationException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(
            string message,
            IDictionary<string, string[]> errors)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>(errors);
        }
    }
}
