using Restaurant.Domain.Entities;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Infrastructure.Mappings
{
    internal static class Extensions
    {
        public static Addition AsEntity(this AdditionPoco additionPoco)
        {
            return new Addition(additionPoco.Id, additionPoco.AdditionName, additionPoco.Price, additionPoco.AdditionKind, additionPoco.ProductSales?.Select(p => new EntityId(p.Id)).ToList());
        }

        public static AdditionPoco AsPoco(this Addition addition)
        {
            return new AdditionPoco
            {
                Id = addition.Id,
                AdditionName = addition.AdditionName,
                Price = addition.Price,
                AdditionKind = addition.AdditionKind
            };
        }

        public static Product AsEntity(this ProductPoco productPoco)
        {
            return new Product(productPoco.Id, productPoco.ProductName, productPoco.Price, productPoco.ProductKind,
                productPoco.ProductSales?.Select(o => o.Order?.AsEntity()),
                productPoco.ProductSales?.Select(p => new EntityId(p.Id)));
        }

        public static ProductPoco AsPoco(this Product product)
        {
            return new ProductPoco
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductKind = product.ProductKind,
                ProductSales = product.Orders.ToProductSalePoco()
            };
        }

        private static IEnumerable<ProductSalePoco> ToProductSalePoco(this IEnumerable<Order> orders)
        {
            var list = new List<ProductSalePoco>();

            foreach (var order in orders)
            {
                if (order is null)
                {
                    continue;
                }

                list.AddRange(order.Products.Select(p => p.AsPoco()));
            }

            return list;
        }

        public static Order AsEntity(this OrderPoco orderPoco)
        {
            return new Order(orderPoco.Id, orderPoco.OrderNumber, orderPoco.Created, orderPoco.Price,
                Email.Of(orderPoco.Email), orderPoco.Note, orderPoco.Products?.Select(p => p?.AsEntity()));
        }

        public static OrderPoco AsPoco(this Order order)
        {
            return new OrderPoco
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Created = order.Created,
                Email = order.Email.Value,
                Note = order.Note,
                Price = order.Price,
                Products = order.Products?.Select(p => p?.AsPoco())
            };
        }

        public static ProductSale AsEntity(this ProductSalePoco productSalePoco)
        {
            return new ProductSale(productSalePoco.Id, productSalePoco.Product.AsEntity(), productSalePoco.ProductSaleState,
                Email.Of(productSalePoco.Email), productSalePoco.Addition?.AsEntity(), productSalePoco.Order?.AsEntity());
        }

        public static ProductSalePoco AsPoco(this ProductSale productSale)
        {
            return new ProductSalePoco
            {
                Id = productSale.Id,
                Email = productSale.Email.Value,
                EndPrice = productSale.EndPrice,
                Addition = productSale.Addition?.AsPoco(),
                Order = productSale.Order?.AsPoco(),
                Product = productSale.Product.AsPoco(),
                ProductSaleState = productSale.ProductSaleState
            };
        }

        public static User AsEntity(this UserPoco userPoco)
        {
            return new User(userPoco.Id, Email.Of(userPoco.Email), userPoco.Password, userPoco.Role, userPoco.CreatedAt);
        }

        public static UserPoco AsPoco(this User user)
        {
            return new UserPoco
            {
                Id = user.Id,
                Email = user.Email.Value,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
        }
    }
}
