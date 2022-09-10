namespace Restaurant.Domain.Exceptions
{
    public sealed class InvalidProductKindException : DomainException
    {
        public string ProductKind { get; }

        public InvalidProductKindException(string productKind) : base($"Invalid ProductKind: '{productKind}'")
        {
            ProductKind = productKind;
        }
    }
}
