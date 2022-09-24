using NHibernate;
using NHibernate.Engine;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using Restaurant.Domain.Entities;
using Restaurant.Domain.ValueObjects;
using Restaurant.Infrastructure.Mappings;
using System.Data.Common;

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
                map.Column(nameof(AdditionPoco.Id));
                map.Type<EntityMapImmutableType>();
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
            /*Bag(o => o.ProductSales, map =>
            {
                map.Table("ProductSales");
                map.Key(k => k.Column(col => col.Name("AdditionId")));
            }, map => map.OneToMany());*/
        }
    }

    public sealed class EntityMapImmutableType : IUserType
    {
        public SqlType[] SqlTypes => new[] { SqlTypeFactory.Guid };

        public Type ReturnedType => typeof(EntityMapImmutableType);

        public bool IsMutable => false;

        public object Assemble(object cached, object owner)
            => DeepCopy(cached);

        public object DeepCopy(object value)
            => value;

        public object Disassemble(object value)
            => DeepCopy(value);

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
            => x.GetHashCode();

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var obj = (Guid)NHibernateUtil.Guid.NullSafeGet(rs, names[0], session);
            if (obj == Guid.Empty) return null;
            return new EntityId(obj);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var type = value.GetType();

            if (type == typeof(Guid))
            {
                NHibernateUtil.Guid.NullSafeSet(cmd, value, index, session);
                return;
            }

            EntityId entityId = value as EntityId;
            object valueToSet;

            if (entityId != null)
            {
                valueToSet = entityId.Value;
            }
            else
            {
                valueToSet = DBNull.Value;
            }

            NHibernateUtil.Guid.NullSafeSet(cmd, valueToSet, index, session);
        }

        public object Replace(object original, object target, object owner)
            => original;
    }
}
