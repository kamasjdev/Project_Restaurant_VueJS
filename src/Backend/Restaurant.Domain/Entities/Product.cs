using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Product
    {
        public EntityId Id { get; }
        public ProductName ProductName { get; private set; }
        public Price Price { get; private set; }
        public ProductKind ProductKind { get; set; }

        public IEnumerable<Order> Orders => _orders;
        private IList<Order> _orders = new List<Order>();

        public Product(EntityId id, ProductName productName, Price price, ProductKind productKind, IEnumerable<Order> orders = null)
        {
            Id = id;
            ChangeProductName(productName);
            ChangePrice(price);
            _orders = orders.ToList() ?? new List<Order>();
            ProductKind = productKind;
        }

        public void ChangePrice(Price price)
        {
            Price = price;
        }

        public void ChangeProductName(ProductName productName)
        {
            ProductName = productName;
        }

        public void AddOrders(IEnumerable<Order> orders)
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

        public void AddOrder(Order order)
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
    }
}
