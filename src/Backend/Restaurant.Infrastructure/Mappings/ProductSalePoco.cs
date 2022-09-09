using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Mappings
{
    public class ProductSalePoco
    {
        public virtual Guid Id { get; set; }
        public virtual OrderPoco Order { get; set; }
        public virtual ProductPoco Product { get; set; }
        public virtual AdditionPoco Addition { get; set; }
        public virtual string Email { get; set; }
        public virtual decimal EndPrice { get; set; }
        public virtual ProductSaleState ProductSaleState { get; set; }
    }
}
