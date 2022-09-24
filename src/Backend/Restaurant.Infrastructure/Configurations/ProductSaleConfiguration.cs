using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Configurations
{
    public class ProductSaleConfiguration : ClassMapping<ProductSalePoco>
    {
        public ProductSaleConfiguration()
        {
            Table("ProductSales");
            Id(p => p.Id, map => map.Column(nameof(ProductSalePoco.Id)));
            Property(p => p.Email, map => map.Column(nameof(ProductSalePoco.Email)));
            Property(p => p.EndPrice, map => map.Column(nameof(ProductSalePoco.EndPrice)));
            Property(p => p.ProductSaleState, map => {
                map.Column(nameof(ProductSalePoco.ProductSaleState));
                map.Type<EnumStringType<ProductSaleState>>();
            });
            ManyToOne(p => p.Order, map =>
            {
                map.Column("OrderId");
            });
            ManyToOne(p => p.Product, map =>
            {
                map.Column("ProductId");
            });
            ManyToOne(p => p.Addition, map =>
            {
                map.Column("AdditionId");
            });
        }
    }

    /*public class ProductSaleConfig : ClassMap<ProductSale>
    {
        public ProductSaleConfig()
        {
            Table("ProductSales");
            Id(a => a.Id).Column(nameof(Addition.Id));
            CompositeId(a => a.Id)
                .KeyProperty(ad => ad.Value, k =>
                {
                    k.ColumnName(nameof(Addition.Id));
                    k.Access.CamelCaseField(Prefix.Underscore);
                });
            Component(p => p.Email, email =>
            {
                email.Map(em => em.Value, nameof(ProductSale.Email));
            });
            Component(a => a.EndPrice, ad =>
            {
                ad.Map(name => name.Value, nameof(ProductSale.EndPrice));
            });
            Map(a => a.ProductSaleState).CustomType<EnumStringType<ProductSaleState>>()
                .Column(nameof(ProductSale.ProductSaleState));
            References(p => p.Order).Column("OrderId");
            References(p => p.Product).Column("ProductId");
            References(p => p.Addition).Column("AdditionId");
        }
    }*/
}
