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

        private IList<ProductSale> _productSaleIds = new List<ProductSale>();
        public virtual IEnumerable<ProductSale> ProductSales { get { return _productSaleIds; } protected set { _productSaleIds = value.ToList(); } }

        protected Addition() { }

        public Addition(EntityId id, AdditionName additionName, Price price, AdditionKind additionKind, IEnumerable<ProductSale> productSales = null)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            AdditionKind = additionKind;
            _productSaleIds = productSales?.ToList() ?? new List<ProductSale>();
        }

        public Addition(EntityId id, AdditionName additionName, Price price, string additionKind, IEnumerable<ProductSale> productSales = null)
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
            _productSaleIds = productSales?.ToList() ?? new List<ProductSale>();
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