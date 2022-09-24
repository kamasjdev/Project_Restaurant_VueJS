using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Addition
    {
        public virtual EntityId Id { get; protected set; }
        public virtual AdditionName AdditionName { get; protected set; }
        public virtual Price Price { get; protected set; }
        public virtual AdditionKind AdditionKind { get; protected set; }

        private IList<EntityId> _productSaleIds = new List<EntityId>();
        public virtual IEnumerable<EntityId> ProductSaleIds => _productSaleIds;

        protected Addition() { }

        public Addition(EntityId id, AdditionName additionName, Price price, AdditionKind additionKind, IEnumerable<EntityId> productSaleIds = null)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            AdditionKind = additionKind;
            _productSaleIds = productSaleIds?.ToList() ?? new List<EntityId>();
        }

        public Addition(EntityId id, AdditionName additionName, Price price, string additionKind, IEnumerable<EntityId> productSaleIds = null)
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
            _productSaleIds = productSaleIds?.ToList() ?? new List<EntityId>();
        }

        public virtual void ChangeAdditionName(AdditionName additionName)
        {
            AdditionName = additionName;
        }

        public virtual void ChangePrice(Price price)
        {
            Price = price;
        }
    }
}