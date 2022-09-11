using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092045)]
    public sealed class AddOrderTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Orders");
        }

        public override void Up()
        {
            Create.Table("Orders")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("OrderNumber").AsString(200)
                .WithColumn("Price").AsDecimal()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Email").AsString(200)
                .WithColumn("Note").AsString(5000).NotNullable();
        }
    }
}
