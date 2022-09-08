namespace Restaurant.Domain.Exceptions
{
    public sealed class EmailCannotBeEmptyException : DomainException
    {
        public EmailCannotBeEmptyException() : base("Email cannot be empty")
        {
        }
    }
}
