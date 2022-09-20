using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202209202030)]
    public sealed class AddDataToUserTable : Migration
    {
        public override void Down()
        {
            Delete.FromTable("users")
                .Row(new { Id = new Guid("00000000-0000-0000-0000-000000000001") });
        }

        public override void Up()
        {
            Insert.IntoTable("Users")
                .Row(new Dictionary<string, object>
                {
                    { "Id", new Guid("00000000-0000-0000-0000-000000000001") },
                    { "Email", "admin@admin.com" },
                                // PasW0Rd!26
                    { "Password", "AQAAAAEAACcQAAAAEGSTX4wH4RpTdpPPUHA22VqiKSiOXtu0n5xjyUuGLk0Q80FKnySV9cwELA3BivIX4g==" },
                    { "Role", "admin" },
                    { "CreatedAt", new DateTime(2022, 9, 20, 20, 30, 10) }
                });
        }
    }
}
