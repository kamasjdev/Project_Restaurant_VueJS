using NHibernate.Mapping.ByCode.Conformist;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Configurations
{
    public sealed class UserConfiguration : ClassMapping<UserPoco>
    {
        public UserConfiguration()
        {
            Table("Users");
            Id(o => o.Id, map => map.Column(nameof(UserPoco.Id)));
            Property(o => o.Email, map => map.Column(nameof(UserPoco.Email)));
            Property(o => o.Password, map => map.Column(nameof(UserPoco.Password)));
            Property(o => o.CreatedAt, map => map.Column(nameof(UserPoco.CreatedAt)));
            Property(o => o.Role, map => map.Column(nameof(UserPoco.Role)));
        }
    }
}
