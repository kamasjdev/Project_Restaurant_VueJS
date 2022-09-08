using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092055)]
    public sealed class AddProductSaleTable : Migration
    {
        public override void Down()
        {
            Delete.Table("ProductSales");
        }

        public override void Up()
        {
            Create.Table("ProductSales")
                  .WithColumn("Id").AsGuid().PrimaryKey()
                  .WithColumn("OrderId").AsGuid().ForeignKey("Orders", "Id")
                  .WithColumn("ProductId").AsGuid().ForeignKey("Products", "Id")
                  .WithColumn("Email").AsString(200)
                  .WithColumn("EndPrice").AsDecimal()
                  .WithColumn("ProductSaleState").AsString(10);
        }
    }
}
