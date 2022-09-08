using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Mappings
{
    public class ProductSalePoco
    {
        public Guid Id { get; set; }
        public OrderPoco Order { get; set; }
        public ProductPoco Product { get; set; }
        public AdditionPoco Addition { get; set; }
        public string Email { get; set; }
        public decimal EndPrice { get; set; }
        public ProductSaleState ProductSaleState { get; set; }
    }
}
