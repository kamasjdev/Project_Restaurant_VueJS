using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class ProductSale
    {
        public virtual EntityId Id { get; protected set; }
        public virtual EntityId ProductId => Product?.Id;
        public virtual Product Product { get; protected set; }
        public virtual EntityId AdditionId => Addition?.Id;
        public virtual Addition Addition { get; protected set; } = null;
        public virtual Price EndPrice { get; protected set; } = decimal.Zero;
        public virtual EntityId OrderId => Order?.Id;
        public virtual Order Order { get; protected set; } = null;
        public virtual ProductSaleState ProductSaleState { get; protected set; } = ProductSaleState.New;
        public virtual Email Email { get; protected set; }

        protected ProductSale() { }

        public ProductSale(EntityId id, Product product, ProductSaleState productSaleState, Email email, Addition addition = null, Order order = null)
        {
            Id = id;
            ChangeProduct(product);

            if (addition != null)
            {
                ChangeAddition(addition);
            }

            Order = order;
            ProductSaleState = productSaleState;
            Email = email;
        }

        public virtual void ChangeProduct(Product product)
        {
            if (product is null)
            {
                throw new ProductCannotBeNullException();
            }

            if (Product != null)
            {
                EndPrice -= Product.Price;
            }

            Product = product;
            EndPrice += product.Price;
        }

        public virtual void ChangeAddition(Addition addition)
        {
            if (addition is null)
            {
                throw new AdditionCannotBeNullException();
            }

            if (Addition != null)
            {
                EndPrice -= Addition.Price;
            }

            Addition = addition;
            EndPrice += addition.Price;
        }

        public virtual void RemoveAddition()
        {
            if (Addition is null)
            {
                throw new AdditionNotExistsException(Id);
            }

            EndPrice -= Addition.Price;
            Addition = null;
        }

        public virtual void AddOrder(Order order)
        {
            if (order is null)
            {
                throw new OrderCannotBeNullException();
            }

            if (Order != null)
            {
                throw new CannotOverrideExistingOrderException(Id);
            }

            Order = order;
            ProductSaleState = ProductSaleState.Ordered;
        }

        public virtual void RemoveOrder()
        {
            if (Order is null)
            {
                throw new OrderNotExsitsException(Id);
            }

            Order = null;
            ProductSaleState = ProductSaleState.New;
        }

        public virtual void ChangeEmail(Email email)
        {
            Email = email;
        }
    }
}
