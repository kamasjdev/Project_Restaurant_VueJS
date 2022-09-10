using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Addition
    {
        public EntityId Id { get; }
        public AdditionName AdditionName { get; private set; }
        public Price Price { get; private set; }
        public AdditionKind AdditionKind { get; }

        public Addition(EntityId id, AdditionName additionName, Price price, AdditionKind additionKind)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            AdditionKind = additionKind;
        }

        public Addition(EntityId id, AdditionName additionName, Price price, string additionKind)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            var parsed = Enum.TryParse<AdditionKind>(additionKind, out var additionKindParsed);

            if (!parsed)
            {
                throw new InvalidAdditionKindException(additionKind);
            }

            if (!Enum.IsDefined(additionKindParsed))
            {
                throw new InvalidAdditionKindException(additionKind);
            }

            AdditionKind = additionKindParsed;
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