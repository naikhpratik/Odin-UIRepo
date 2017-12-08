namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderInfoToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "OrderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "OrderId");
            AddForeignKey("dbo.Notifications", "OrderId", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "OrderId", "dbo.Orders");
            DropIndex("dbo.Notifications", new[] { "OrderId" });
            DropColumn("dbo.Notifications", "OrderId");
        }
    }
}
