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
        }
    }
}
