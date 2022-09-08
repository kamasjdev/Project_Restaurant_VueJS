using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public sealed class OrderNumber
    {
        public string Value { get; }

        public OrderNumber(string productName)
        {
            ValidProductName(productName);
            Value = productName;
        }

        public static implicit operator string(OrderNumber orderNumber)
            => orderNumber.Value;

        public static implicit operator OrderNumber(string value)
            => new(value);

        private static void ValidProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new OrderNumberCannotBeEmptyException();
            }
        }
    }
}
