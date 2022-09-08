using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092005)]
    public sealed class AddAdditionTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Additions");
        }

        public override void Up()
        {
            Create.Table("Additions")
               .WithColumn("Id").AsGuid().PrimaryKey()
               .WithColumn("AdditionName").AsString(200)
               .WithColumn("Price").AsDecimal()
               .WithColumn("ProductKind").AsString(10);
        }
    }
}