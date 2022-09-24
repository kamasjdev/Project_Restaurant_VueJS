using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Order
    {
        public virtual EntityId Id { get; protected set; }
        public virtual OrderNumber OrderNumber { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual Price Price { get; protected set; }
        public virtual Email Email { get; protected set; }
        public virtual string Note { get; protected set; } = null;

        public virtual IEnumerable<ProductSale> Products { get { return _products; } protected set { _products = value.ToList(); } }
        private IList<ProductSale> _products = new List<ProductSale>();

        protected Order() { }

        public Order(EntityId id, OrderNumber orderNumber, DateTime created, Price price, Email email, string note = null, IEnumerable<ProductSale> products = null)
        {
            Id = id;
            ChangeOrderNumber(orderNumber);
            Created = created;
            ChangePrice(price);
            ChangeEmail(email);

            if (products != null)
            {
                AddProducts(products);
            }

            Note = note;
        }

        public virtual void ChangeOrderNumber(string orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public virtual void ChangePrice(decimal price)
        {
            Price = price;
        }

        public virtual void ChangeEmail(Email email)
        {
            Email = email;
        }

        public virtual void ChangeNote(string note)
        {
            Note = note;
        }

        public virtual void AddProducts(IEnumerable<ProductSale> products)
        {
            if (products is null)
            {
                throw new ProductCannotBeNullException();
            }

            foreach (var product in products)
            {
                AddProduct(product);
            }
        }

        public virtual void AddProduct(ProductSale product)
        {
            if (product is null)
            {
                throw new ProductCannotBeNullException();
            }

            var productToAdd = _products.Where(p => p.Id == product.Id).SingleOrDefault();

            if (productToAdd != null)
            {
                throw new ProductAlreadyExistsException(productToAdd.Id);
            }

            _products.Add(product);
            product.AddOrder(this);
        }

        public virtual void RemoveProduct(ProductSale product)
        {
            if (product is null)
            {
                throw new ProductCannotBeNullException();
            }

            var productToDelete = _products.Where(p => p.Id == product.Id).SingleOrDefault();

            if (productToDelete is null)
            {
                throw new ProductNotFoundException(product.Id);
            }

            _products.Remove(product);
            product.RemoveOrder();
        }
    }
}
