namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderTransfereeAndRentObjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConsultantAssignments",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        ConsultantId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.OrderId, t.ConsultantId })
                .ForeignKey("dbo.AspNetUsers", t => t.ConsultantId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.ConsultantId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrackingId = c.String(),
                        RelocationType = c.String(),
                        DestinationCity = c.String(),
                        DestinationState = c.String(),
                        DestinationZip = c.String(),
                        DestinationCountry = c.String(),
                        OriginCity = c.String(),
                        OriginState = c.String(),
                        OriginCountry = c.String(),
                        RentId = c.Int(),
                        TransfereeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transferees", t => t.TransfereeId)
                .Index(t => t.TransfereeId);
            
            CreateTable(
                "dbo.Rents",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HousingBudget = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Transferees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        SpouseName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "TransfereeId", "dbo.Transferees");
            DropForeignKey("dbo.Rents", "Id", "dbo.Orders");
            DropForeignKey("dbo.ConsultantAssignments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.ConsultantAssignments", "ConsultantId", "dbo.AspNetUsers");
            DropIndex("dbo.Rents", new[] { "Id" });
            DropIndex("dbo.Orders", new[] { "TransfereeId" });
            DropIndex("dbo.ConsultantAssignments", new[] { "ConsultantId" });
            DropIndex("dbo.ConsultantAssignments", new[] { "OrderId" });
            DropTable("dbo.Transferees");
            DropTable("dbo.Rents");
            DropTable("dbo.Orders");
            DropTable("dbo.ConsultantAssignments");
        }
    }
}
