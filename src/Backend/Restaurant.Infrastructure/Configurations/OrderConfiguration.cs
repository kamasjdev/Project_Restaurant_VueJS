using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Configurations
{
    public class OrderConfiguration : ClassMapping<OrderPoco>
    {
        public OrderConfiguration()
        {
            Table("Orders");
            Id(o => o.Id, map => map.Column(nameof(ProductPoco.Id)));
            Property(o => o.OrderNumber, map => map.Column(nameof(OrderPoco.OrderNumber)));
            Property(o => o.Price, map => map.Column(nameof(OrderPoco.Price)));
            Property(o => o.Created, map =>
            { 
                map.Column(nameof(OrderPoco.Created));
            });
            Property(o => o.Email, map => map.Column(nameof(OrderPoco.Email)));
            Property(o => o.Note, map => map.Column(nameof(OrderPoco.Note)));
            Bag(o => o.Products, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("OrderId")));
            }, map => map.OneToMany());
        }
    }
}
