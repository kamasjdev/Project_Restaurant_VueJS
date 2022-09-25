using NHibernate.Collection.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Configurations
{
    public class ProductConfiguration : ClassMapping<ProductPoco>
    {
        public ProductConfiguration()
        {
            Table("Products");
            Id(p => p.Id, map => map.Column(nameof(ProductPoco.Id)));
            Property(p => p.ProductName, map => map.Column(nameof(ProductPoco.ProductName)));
            Property(p => p.Price, map => map.Column(nameof(ProductPoco.Price)));
            Property(p => p.ProductKind, map => {
                map.Column(nameof(ProductPoco.ProductKind));
                map.Type<EnumStringType<ProductKind>>();
            });
            Bag(o => o.ProductSales, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("ProductId")));
            }, map => map.OneToMany());
        }
    }

    public class ProductConfig : ClassMapping<Product>
    {
        public ProductConfig()
        {
            Table("Products");
            Id(p => p.Id, map =>
            {
                map.Column(nameof(ProductSale.Id));
                map.Type<EntityIdConfigurationType>();
            });
            Component(p => p.ProductName, map =>
            {
                map.Property(p => p.Value, prop =>
                {
                    prop.Column(nameof(Product.ProductName));
                });
            });
            Component(p => p.Price, map =>
            {
                map.Property(p => p.Value, prop =>
                {
                    prop.Column(nameof(Product.Price));
                });
            });
            Property(p => p.ProductKind, map => {
                map.Column(nameof(ProductPoco.ProductKind));
                map.Type<EnumStringType<ProductKind>>();
            });
            Bag(o => o.Orders, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("ProductId")));
            }, map => map.ManyToMany(many => many.Column("OrderId")));;
        }
    }
}
