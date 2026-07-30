namespace NotaLink.Application.Exceptions
{
    public class ValidationErrorException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationErrorException(Dictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
