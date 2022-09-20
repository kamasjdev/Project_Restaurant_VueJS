namespace Restaurant.Domain.Exceptions
{
    public sealed class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() : base($"Invalid password. Password should have at least 8 characters, including upper letter and number")
        {
        }
    }
}
