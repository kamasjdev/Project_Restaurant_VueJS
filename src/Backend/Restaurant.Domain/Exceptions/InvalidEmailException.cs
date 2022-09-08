namespace Restaurant.Domain.Exceptions
{
    public sealed class InvalidEmailException : DomainException
    {
        public string Email { get; }

        public InvalidEmailException(string email) : base($"Invalid Email: '{email}'")
        {
            Email = email;
        }
    }
}
