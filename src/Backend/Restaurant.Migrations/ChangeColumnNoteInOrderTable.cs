using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202209101730)]
    public class ChangeColumnNoteInOrderTable : Migration
    {
        public override void Down()
        {
            Create.Table("Orders_backup")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("OrderNumber").AsString(200)
                .WithColumn("Price").AsDecimal()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Email").AsString(200)
                .WithColumn("Note").AsString(5000).Nullable();
            Execute.Sql(@"INSERT INTO Orders_backup (Id, OrderNumber, Price, Created, Email, Note)
                          SELECT * FROM Orders");
            Delete.Table("Orders");
            Create.Table("Orders")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("OrderNumber").AsString(200)
                .WithColumn("Price").AsDecimal()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Email").AsString(200)
                .WithColumn("Note").AsString(5000).NotNullable();
            Create.Index("idx_orders_created").OnTable("Orders").OnColumn("Created");
            Create.Index("idx_orders_email").OnTable("Orders").OnColumn("Email");
            Create.Index("uidx_orders_order_number").OnTable("Orders").OnColumn("OrderNumber").Unique();
            Execute.Sql(@"INSERT INTO Orders (Id, OrderNumber, Price, Created, Email, Note)
                          SELECT Id, OrderNumber, Price, Created, Email, CASE WHEN Note IS NULL THEN '' ELSE Note END 
                          FROM Orders_backup");
            Delete.Table("Orders_backup");
        }

        public override void Up()
        {
            Create.Table("Orders_backup")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("OrderNumber").AsString(200)
                .WithColumn("Price").AsDecimal()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Email").AsString(200)
                .WithColumn("Note").AsString(5000).NotNullable();
            Execute.Sql(@"INSERT INTO Orders_backup (Id, OrderNumber, Price, Created, Email, Note)
                          SELECT * FROM Orders");
            Delete.Table("Orders");
            Create.Table("Orders")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("OrderNumber").AsString(200)
                .WithColumn("Price").AsDecimal()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Email").AsString(200)
                .WithColumn("Note").AsString(5000).Nullable();
            Create.Index("idx_orders_created").OnTable("Orders").OnColumn("Created");
            Create.Index("idx_orders_email").OnTable("Orders").OnColumn("Email");
            Create.Index("uidx_orders_order_number").OnTable("Orders").OnColumn("OrderNumber").Unique();
            Execute.Sql(@"INSERT INTO Orders (Id, OrderNumber, Price, Created, Email, Note)
                          SELECT * FROM Orders_backup");
            Delete.Table("Orders_backup");
        }
    }
}
