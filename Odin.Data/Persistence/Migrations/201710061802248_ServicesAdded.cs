namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServicesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ScheduledDate = c.DateTime(),
                        CompletedDate = c.DateTime(),
                        Order_Id = c.Int(),
                        ServiceType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.ServiceTypes", t => t.ServiceType_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.ServiceType_Id);
            
            CreateTable(
                "dbo.ServiceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "ServiceType_Id", "dbo.ServiceTypes");
            DropForeignKey("dbo.Services", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Services", new[] { "ServiceType_Id" });
            DropIndex("dbo.Services", new[] { "Order_Id" });
            DropTable("dbo.ServiceTypes");
            DropTable("dbo.Services");
        }
    }
}
