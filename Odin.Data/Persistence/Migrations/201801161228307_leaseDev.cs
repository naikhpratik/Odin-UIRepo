namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class leaseDev : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leases",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        PropertyId = c.String(maxLength: 128),
                        Tenant = c.String(),
                        LandLord = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        RentIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SecurityDeposit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SecurityDepositTerms = c.String(),
                        LeaseEndNoticeTerms = c.String(),
                        RenewalTerms = c.String(),
                        DiplomatTerms = c.String(),
                        EarlyTerminationTerms = c.String(),
                        NotableClauses = c.String(),
                        PaymentInformation = c.String(),
                        InitialRentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InitialRentDueDate = c.DateTime(),
                        InitialRentPaideTo = c.String(),
                        SecurityDepositDueDate = c.DateTime(),
                        SecurityDepositPaideTo = c.String(),
                        FirstOnGoingRentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FirstOnGoingRentDueDate = c.DateTime(),
                        FirstOnGoingRentPaideTo = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                        transferee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.PropertyId)
                .ForeignKey("dbo.AspNetUsers", t => t.transferee_Id)
                .Index(t => t.PropertyId)
                .Index(t => t.CreatedAt, clustered: true)
                .Index(t => t.transferee_Id);
            
            AddColumn("dbo.HomeFindingProperties", "selected", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Leases", "transferee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Leases", "PropertyId", "dbo.Properties");
            DropIndex("dbo.Leases", new[] { "transferee_Id" });
            DropIndex("dbo.Leases", new[] { "CreatedAt" });
            DropIndex("dbo.Leases", new[] { "PropertyId" });
            DropColumn("dbo.HomeFindingProperties", "selected");
            DropTable("dbo.Leases",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
        }
    }
}
