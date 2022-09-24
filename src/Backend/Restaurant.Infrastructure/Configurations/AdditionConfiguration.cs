using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Configurations
{
    public sealed class AdditionConfiguration : ClassMapping<AdditionPoco>
    {
        public AdditionConfiguration()
        {
            Table("Additions");
            Id(a => a.Id, map => map.Column(nameof(AdditionPoco.Id)));
            Property(a => a.AdditionName, map => map.Column(nameof(AdditionPoco.AdditionName)));
            Property(a => a.Price, map => map.Column(nameof(AdditionPoco.Price)));
            Property(a => a.AdditionKind, map => {
                map.Column(nameof(AdditionPoco.AdditionKind));
                map.Type<EnumStringType<AdditionKind>>();
            });
            Bag(o => o.ProductSales, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("AdditionId")));
            }, map => map.OneToMany());
        }
    }
    
    public sealed class AdditionConfig : ClassMapping<Addition>
    {
        public AdditionConfig()
        {
            Table("Additions");
            Id(a => a.Id, map => 
            {
                map.Column(nameof(Addition.Id));
                map.Type<EntityIdConfigurationType>();
            });
            Component(a => a.AdditionName, map =>
            {
                map.Property(ad => ad.Value, prop =>
                {
                    prop.Column(nameof(Addition.AdditionName));
                });
            });
            Component(a => a.Price, map =>
            {
                map.Property(p => p.Value, prop =>
                {
                    prop.Column(nameof(Addition.Price));
                });
            });
            Property(a => a.AdditionKind, map => map.Type<EnumStringType<AdditionKind>>());
            Bag(o => o.ProductSales, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("AdditionId")));
            }, map => map.OneToMany());
        }
    }
}
