namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyOnServices : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Services", new[] { "Order_Id" });
            RenameColumn(table: "dbo.Services", name: "Order_Id", newName: "OrderId");
            AlterColumn("dbo.Services", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "OrderId");
            AddForeignKey("dbo.Services", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "OrderId", "dbo.Orders");
            DropIndex("dbo.Services", new[] { "OrderId" });
            AlterColumn("dbo.Services", "OrderId", c => c.Int());
            RenameColumn(table: "dbo.Services", name: "OrderId", newName: "Order_Id");
            CreateIndex("dbo.Services", "Order_Id");
            AddForeignKey("dbo.Services", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
