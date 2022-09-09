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
}
