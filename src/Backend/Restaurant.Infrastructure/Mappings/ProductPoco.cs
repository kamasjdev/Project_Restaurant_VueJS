using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Mappings
{
    public class ProductPoco
    {
        public virtual Guid Id { get; set; }
        public virtual string ProductName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual ProductKind ProductKind { get; set; }
        public virtual IEnumerable<ProductSalePoco> ProductSales { get; set; }
    }
}
