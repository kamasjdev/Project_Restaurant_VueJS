using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Exceptions
{
    public sealed class CannotOverrideExistingOrderException : DomainException
    {
        public EntityId ProductSaleId { get; }

        public CannotOverrideExistingOrderException(EntityId productSaleId) : base($"Cannot override existing Order in ProductSale: '{productSaleId}'")
        {
            ProductSaleId = productSaleId;
        }
    }
}
