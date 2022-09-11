using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Mappings
{
    public class AdditionPoco
    {
        public virtual Guid Id { get; set; }
        public virtual string AdditionName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual AdditionKind AdditionKind { get; set; }
        public virtual IEnumerable<ProductSalePoco> ProductSales { get; set; }
    }
}
