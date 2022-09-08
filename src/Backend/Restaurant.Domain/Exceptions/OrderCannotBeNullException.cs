namespace Restaurant.Domain.Exceptions
{
    public sealed class OrderCannotBeNullException : DomainException
    {
        public OrderCannotBeNullException() : base("Order cannot be null")
        {
        }
    }
}
