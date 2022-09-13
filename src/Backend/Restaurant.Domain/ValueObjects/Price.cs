using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public class Price
    {
        public decimal Value { get; }

        protected Price() { }

        public Price(decimal price)
        {
            ValidPrice(price);
            Value = price;
        }

        public static implicit operator decimal(Price additionName)
            => additionName.Value;

        public static implicit operator Price(decimal value)
            => new(value);

        private static void ValidPrice(decimal price)
        {
            if (price < 0)
            {
                throw new PriceCannotBeNegativeException(price);
            }
        }
    }
}
