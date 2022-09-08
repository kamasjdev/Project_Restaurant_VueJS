namespace Restaurant.Infrastructure.Mappings
{
    public class OrderPoco
    {
        public virtual Guid Id { get; set; }
        public virtual string OrderName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual string Email { get; set; }
        public virtual string Note { get; set; }
        public virtual IEnumerable<ProductSalePoco> Products { get; set; }
    }
}
