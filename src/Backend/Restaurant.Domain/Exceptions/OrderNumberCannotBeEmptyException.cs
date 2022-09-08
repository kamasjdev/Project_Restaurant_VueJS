namespace Restaurant.Domain.Exceptions
{
    public sealed class OrderNumberCannotBeEmptyException : DomainException
    {
        public OrderNumberCannotBeEmptyException() : base("OrderNumber cannot be empty")
        {
        }
    }
}
