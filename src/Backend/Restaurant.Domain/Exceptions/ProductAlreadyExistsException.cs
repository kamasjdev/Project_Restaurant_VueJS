using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Exceptions
{
    public sealed class ProductAlreadyExistsException : DomainException
    {
        public EntityId ProductId { get; }

        public ProductAlreadyExistsException(EntityId productId) : base($"Product with id: '{productId}' already exists")
        {
            ProductId = productId;
        }
    }
}
