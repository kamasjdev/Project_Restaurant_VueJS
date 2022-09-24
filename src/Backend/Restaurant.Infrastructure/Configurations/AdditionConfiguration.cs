using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Restaurant.Domain.Entities;
using Restaurant.Domain.ValueObjects;
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
            Property(a => a.AdditionKind, map =>
            {
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

    public sealed class AdditionConfig : ClassMap<Addition>
    {
        public AdditionConfig()
        {
            Table("Additions");
            Id(a => a.Id).Column(nameof(Addition.Id));
            CompositeId(a => a.Id)
                .KeyProperty(ad => ad.Value, k => 
                {
                    k.ColumnName(nameof(Addition.Id));
                    k.Access.CamelCaseField(Prefix.Underscore);
                });
            Component(a => a.AdditionName, ad =>
            {
                ad.Map(name => name.Value, nameof(AdditionName));
            });
            Component(a => a.Price, ad =>
            {
                ad.Map(name => name.Value, nameof(Price));
            });
            Map(a => a.AdditionKind).CustomType<EnumStringType<AdditionKind>>();
        }
    }

    /*public sealed class EntityIdConfig : ClassMap<EntityId>
    {
        public EntityIdConfig()
        {
            Id(e => e.Value).Column("Id");
        }
    }*/
}
