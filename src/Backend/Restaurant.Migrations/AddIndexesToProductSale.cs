using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092325)]
    public sealed class AddIndexesToProductSale : Migration
    {
        public override void Down()
        {
            Delete.Index("idx_product_sales_product_id").OnTable("ProductSales").OnColumn("ProductId");
            Delete.Index("idx_product_sales_order_id").OnTable("ProductSales").OnColumn("OrderId");
            Delete.Index("idx_product_sales_addition_id").OnTable("ProductSales").OnColumn("AdditionId");
            Delete.Index("idx_product_sales_email").OnTable("ProductSales").OnColumn("Email");
            Delete.Index("idx_product_sales_product_sale_state").OnTable("ProductSales").OnColumn("ProductSaleState");
        }

        public override void Up()
        {
            Create.Index("idx_product_sales_product_id").OnTable("ProductSales").OnColumn("ProductId");
            Create.Index("idx_product_sales_order_id").OnTable("ProductSales").OnColumn("OrderId");
            Create.Index("idx_product_sales_addition_id").OnTable("ProductSales").OnColumn("AdditionId");
            Create.Index("idx_product_sales_email").OnTable("ProductSales").OnColumn("Email");
            Create.Index("idx_product_sales_product_sale_state").OnTable("ProductSales").OnColumn("ProductSaleState");
        }
    }
}
