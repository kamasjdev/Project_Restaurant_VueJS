using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092320)]
    public sealed class AddIndexesToProduct : Migration
    {
        public override void Down()
        {
            Delete.Index("idx_products_product_name").OnTable("Products").OnColumn("ProductName");
            Delete.Index("idx_products_product_kind").OnTable("Products").OnColumn("ProductKind");
        }

        public override void Up()
        {
            Create.Index("idx_products_product_name").OnTable("Products").OnColumn("ProductName");
            Create.Index("idx_products_product_kind").OnTable("Products").OnColumn("ProductKind");
        }
    }
}
