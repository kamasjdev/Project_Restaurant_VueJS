﻿using NHibernate.Mapping.ByCode.Conformist;
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
            Property(a => a.ProductKind, map => {
                map.Column(nameof(AdditionPoco.ProductKind));
                map.Type<EnumStringType<ProductKind>>();
            });
        }
    }
}
