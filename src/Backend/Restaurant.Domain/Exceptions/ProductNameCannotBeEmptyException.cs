namespace Restaurant.Domain.Exceptions
{
    public sealed class ProductNameCannotBeEmptyException : DomainException
    {
        public ProductNameCannotBeEmptyException() : base("ProductName cannot be empty")
        {
        }
    }
}
