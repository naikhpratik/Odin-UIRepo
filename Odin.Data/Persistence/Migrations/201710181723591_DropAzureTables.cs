namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class DropAzureTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ConsultantId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ProgramManagerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rents", "Id", "dbo.Orders");
            DropForeignKey("dbo.Services", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Services", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.Orders", "TransfereeId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", "IX_TrackingID");
            DropIndex("dbo.Orders", new[] { "TransfereeId" });
            DropIndex("dbo.Orders", new[] { "ProgramManagerId" });
            DropIndex("dbo.Orders", new[] { "ConsultantId" });
            DropIndex("dbo.Rents", new[] { "Id" });
            DropIndex("dbo.Services", new[] { "OrderId" });
            DropIndex("dbo.Services", new[] { "ServiceTypeId" });
            DropTable("dbo.Orders",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "TransfereeInviteEnabled",
                        new Dictionary<string, object>
                        {
                            { "Default", "False" },
                        }
                    },
                });
            DropTable("dbo.Rents");
            DropTable("dbo.Services");
            DropTable("dbo.ServiceTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduledDate = c.DateTime(),
                        CompletedDate = c.DateTime(),
                        OrderId = c.Int(nullable: false),
                        ServiceTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rents",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HousingBudget = c.Decimal(precision: 18, scale: 2),
                        NumberOfBedrooms = c.Int(nullable: false),
                        NumberOfBathrooms = c.Int(nullable: false),
                        SquareFootage = c.Int(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        HousingType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrackingId = c.String(nullable: false, maxLength: 20),
                        RelocationType = c.String(),
                        DestinationCity = c.String(),
                        DestinationState = c.String(),
                        DestinationZip = c.String(),
                        DestinationCountry = c.String(),
                        OriginCity = c.String(),
                        OriginState = c.String(),
                        OriginCountry = c.String(),
                        EstimatedArrivalDate = c.DateTime(),
                        PreTripDate = c.DateTime(),
                        TempHousingDays = c.Int(nullable: false),
                        TempHousingEndDate = c.DateTime(),
                        FamilyDetails = c.String(),
                        TransfereeInviteEnabled = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        SeCustNumb = c.String(),
                        Rmc = c.String(),
                        Client = c.String(),
                        RentId = c.Int(),
                        TransfereeId = c.String(nullable: false, maxLength: 128),
                        ProgramManagerId = c.String(nullable: false, maxLength: 128),
                        ConsultantId = c.String(nullable: false, maxLength: 128),
                        LastContactedDate = c.DateTime(),
                        IsRush = c.Boolean(nullable: false),
                        IsVip = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Services", "ServiceTypeId");
            CreateIndex("dbo.Services", "OrderId");
            CreateIndex("dbo.Rents", "Id");
            CreateIndex("dbo.Orders", "ConsultantId");
            CreateIndex("dbo.Orders", "ProgramManagerId");
            CreateIndex("dbo.Orders", "TransfereeId");
            CreateIndex("dbo.Orders", "TrackingId", unique: true, name: "IX_TrackingID");
            AddForeignKey("dbo.Orders", "TransfereeId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Services", "ServiceTypeId", "dbo.ServiceTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Services", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Rents", "Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Orders", "ProgramManagerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Orders", "ConsultantId", "dbo.AspNetUsers", "Id");
        }
    }
}
