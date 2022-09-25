using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Restaurant.Domain.Entities;
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

    public class OrderConfig : ClassMapping<Order>
    {
        public OrderConfig()
        {
            Table("Orders");
            Id(o => o.Id, map =>
            {
                map.Column(nameof(Order.Id));
                map.Type<EntityIdConfigurationType>();
            });
            Component(o => o.OrderNumber, map =>
            {
                map.Property(p => p.Value, prop =>
                {
                    prop.Column(nameof(Order.OrderNumber));
                });
            });
            Component(o => o.Price, map =>
            {
                map.Property(p => p.Value, prop =>
                {
                    prop.Column(nameof(Order.Price));
                });
            });
            Property(o => o.Created, map =>
            {
                map.Column(nameof(OrderPoco.Created));
            });

            Component(o => o.Email, map =>
            {
                map.Property(p => p.Value, prop =>
                {
                    prop.Column(nameof(Order.Email));
                });
            });
            Property(o => o.Note, map => map.Column(nameof(OrderPoco.Note)));
            Bag(o => o.Products, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("OrderId")));
                map.BatchSize(25);
            }, map => map.OneToMany());
        }
    }
}
