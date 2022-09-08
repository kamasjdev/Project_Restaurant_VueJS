using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092035)]
    public sealed class AddProductTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Products");
        }

        public override void Up()
        {
            Create.Table("Products")
               .WithColumn("Id").AsGuid().PrimaryKey()
               .WithColumn("ProductName").AsString(200)
               .WithColumn("Price").AsDecimal()
               .WithColumn("ProductKind").AsString(10);
        }
    }
}
