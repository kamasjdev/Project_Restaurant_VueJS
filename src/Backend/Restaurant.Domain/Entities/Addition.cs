using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Addition
    {
        public EntityId Id { get; }
        public AdditionName AdditionName { get; private set; }
        public Price Price { get; private set; }
        public ProductKind ProductKind { get; }

        private Addition()
        {
        }

        public Addition(EntityId id, AdditionName additionName, Price price, ProductKind additionKind)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            ProductKind = additionKind;
        }

        public void ChangeAdditionName(AdditionName additionName)
        {
            AdditionName = additionName;
        }

        public void ChangePrice(Price price)
        {
            Price = price;
        }
    }
}