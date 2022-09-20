using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202209201605)]
    public sealed class AddUserTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Users");
        }

        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Email").AsString(5000).NotNullable().Unique("uidx_users_email")
                .WithColumn("Password").AsString(50).NotNullable()
                .WithColumn("Role").AsString(50).NotNullable().Indexed("idx_users_role")
                .WithColumn("CreatedAt").AsDateTime().NotNullable().Indexed("idx_users_created_at");
        }
    }
}
