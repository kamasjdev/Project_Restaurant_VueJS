﻿using FluentMigrator;

namespace Restaurant.Migrations
{
    [Migration(202208092305)]
    public sealed class AddIndexesToAddition : Migration
    {
        public override void Down()
        {
            Delete.Index("idx_additions_product_kind").OnTable("Additions").OnColumn("ProductKind");
            Delete.Index("idx_additions_addition_name").OnTable("Additions").OnColumn("AdditionName");
        }

        public override void Up()
        {
            Create.Index("idx_additions_product_kind").OnTable("Additions").OnColumn("ProductKind");
            Create.Index("idx_additions_addition_name").OnTable("Additions").OnColumn("AdditionName");
        }
    }
}
