﻿using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public sealed class ProductSale
    {
        public EntityId Id { get; }
        public EntityId ProductId { get; private set; }
        public Product Product { get; private set; }
        public EntityId AdditionId { get; private set; } = null;
        public Addition Addition { get; private set; } = null;
        public Price EndPrice { get; private set; } = decimal.Zero;
        public EntityId OrderId { get; private set; }
        public Order Order { get; private set; } = null;
        public ProductSaleState ProductSaleState { get; private set; } = ProductSaleState.New;
        public Email Email { get; private set; }

        public ProductSale(EntityId id, Product product, ProductSaleState productSaleState, Email email, Addition addition = null, EntityId orderId = null, Order order = null)
        {
            Id = id;
            ChangeProduct(product);

            if (addition != null)
            {
                ChangeAddition(addition);
            }

            OrderId = orderId;
            Order = order;
            ProductSaleState = productSaleState;
            Email = email;
        }

        public void ChangeProduct(Product product)
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
            ProductId = product.Id;
            EndPrice += product.Price;
        }

        public void ChangeAddition(Addition addition)
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
            AdditionId = addition.Id;
            EndPrice += addition.Price;
        }

        public void RemoveAddition()
        {
            if (Addition is null)
            {
                throw new AdditionNotExistsException(Id);
            }

            EndPrice -= Addition.Price;
            Addition = null;
            AdditionId = null;
        }

        public void AddOrder(Order order)
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
            OrderId = order.Id;
            ProductSaleState = ProductSaleState.Ordered;
        }

        public void RemoveOrder()
        {
            if (Order is null)
            {
                throw new OrderNotExsitsException(Id);
            }

            Order = null;
            OrderId = null;
            ProductSaleState = ProductSaleState.New;
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }
    }
}
