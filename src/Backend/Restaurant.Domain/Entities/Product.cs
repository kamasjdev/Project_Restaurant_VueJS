using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Product
    {
        public virtual EntityId Id { get; protected set; }
        public virtual ProductName ProductName { get; protected set; }
        public virtual Price Price { get; protected set; }
        public virtual ProductKind ProductKind { get; protected set; }

        private IList<Order> _orders = new List<Order>();
        public virtual IEnumerable<Order> Orders { get { return _orders; } 
            protected set 
            {
                if (value.Any())
                {
                    _orders = value.ToList(); 
                }
            } 
        }

        protected Product() { }

        public Product(EntityId id, ProductName productName, Price price, ProductKind productKind, IEnumerable<Order> orders = null)
        {
            Id = id;
            ChangeProductName(productName);
            ChangePrice(price);
            _orders = orders?.ToList() ?? new List<Order>();
            ProductKind = productKind;
        }

        public Product(EntityId id, ProductName productName, Price price, string productKind, IEnumerable<Order> orders = null)
        {
            Id = id;
            ChangeProductName(productName);
            ChangePrice(price);
            _orders = orders?.ToList() ?? new List<Order>();
            ProductKind = ParseToProductKind(productKind);
        }

        public virtual void ChangePrice(Price price)
        {
            Price = price;
        }

        public virtual void ChangeProductName(ProductName productName)
        {
            ProductName = productName;
        }

        public virtual void ChangeProductKind(ProductKind productKind)
        {
            ProductKind = productKind;
        }

        public virtual void ChangeProductKind(string productKind)
        {
            ProductKind = ParseToProductKind(productKind);
        }

        public virtual void AddOrders(IEnumerable<Order> orders)
        {
            if (orders is null)
            {
                throw new OrderCannotBeNullException();
            }

            foreach (var order in orders)
            {
                _orders.Add(order);
            }
        }

        public virtual void AddOrder(Order order)
        {
            if (order is null)
            {
                throw new OrderCannotBeNullException();
            }

            var orderToAdd = _orders.Where(p => p.Id == order.Id).SingleOrDefault();

            if (orderToAdd != null)
            {
                throw new OrderAlreadyExistsException(orderToAdd.Id);
            }

            _orders.Add(order);
        }

        private ProductKind ParseToProductKind(string productKind)
        {
            var parsed = Enum.TryParse<ProductKind>(productKind, out var productKindParsed);

            if (!parsed)
            {
                throw new InvalidProductKindException(productKind);
            }

            if (!Enum.IsDefined(productKindParsed))
            {
                throw new InvalidProductKindException(productKind);
            }

            return productKindParsed;
        }
    }
}
