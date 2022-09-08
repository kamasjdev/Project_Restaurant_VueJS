using NHibernate.Mapping.ByCode.Conformist;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Configurations
{
    public class OrderConfiguration : ClassMapping<OrderPoco>
    {
        public OrderConfiguration()
        {
            Table("Orders");
            Id(p => p.Id, map => map.Column(nameof(ProductPoco.Id)));
            Property(p => p.OrderName, map => map.Column(nameof(OrderPoco.OrderName)));
            Property(p => p.Price, map => map.Column(nameof(OrderPoco.Price)));
            Property(p => p.Created, map => map.Column(nameof(OrderPoco.Created)));
            Property(p => p.Email, map => map.Column(nameof(OrderPoco.Email)));
            Property(p => p.Note, map => map.Column(nameof(OrderPoco.Note)));
        }
    }
}
