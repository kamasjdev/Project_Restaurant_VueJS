using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Restaurant.Domain.ValueObjects;
using System.Data.Common;

namespace Restaurant.Infrastructure.Configurations
{
    public sealed class EntityIdConfigurationType : IUserType
    {
        public SqlType[] SqlTypes => new[] { SqlTypeFactory.Guid };

        public Type ReturnedType => typeof(EntityId);

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
            var obj = NHibernateUtil.Guid.NullSafeGet(rs, names[0], session);
            if (obj is null) 
                return null;
            var id = (Guid)obj;
            if (id == Guid.Empty) return null;
            return new EntityId(id);
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
