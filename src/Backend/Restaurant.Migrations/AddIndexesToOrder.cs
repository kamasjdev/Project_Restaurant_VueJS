using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092315)]
    public sealed class AddIndexesToOrder : Migration
    {
        public override void Down()
        {
            Delete.Index("idx_orders_created").OnTable("Orders").OnColumn("Created");
            Delete.Index("idx_orders_email").OnTable("Orders").OnColumn("Email");
            Delete.Index("uidx_orders_order_number").OnTable("Orders").OnColumn("OrderNumber");
        }

        public override void Up()
        {
            Create.Index("idx_orders_created").OnTable("Orders").OnColumn("Created");
            Create.Index("idx_orders_email").OnTable("Orders").OnColumn("Email");
            Create.Index("uidx_orders_order_number").OnTable("Orders").OnColumn("OrderNumber").Unique();
        }
    }
}
