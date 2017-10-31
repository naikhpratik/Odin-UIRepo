namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class MovedLeaseToOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Leases", "BrokerFeeTypeId", "dbo.BrokerFeeTypes");
            DropForeignKey("dbo.Leases", "DepositTypeId", "dbo.DepositTypes");
            DropForeignKey("dbo.Leases", "Id", "dbo.Orders");
            DropIndex("dbo.Leases", new[] { "Id" });
            DropIndex("dbo.Leases", new[] { "DepositTypeId" });
            DropIndex("dbo.Leases", new[] { "BrokerFeeTypeId" });
            DropIndex("dbo.Leases", new[] { "CreatedAt" });
            AddColumn("dbo.Orders", "DepositTypeId", c => c.Byte());
            AddColumn("dbo.Orders", "LeaseTerm", c => c.Int());
            AddColumn("dbo.Orders", "BrokerFeeTypeId", c => c.Byte());
            AddColumn("dbo.Orders", "LengthOfAssignment", c => c.Int());
            CreateIndex("dbo.Orders", "DepositTypeId");
            CreateIndex("dbo.Orders", "BrokerFeeTypeId");
            AddForeignKey("dbo.Orders", "BrokerFeeTypeId", "dbo.BrokerFeeTypes", "Id");
            AddForeignKey("dbo.Orders", "DepositTypeId", "dbo.DepositTypes", "Id");
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
        
        public override void Down()
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
                        DepositTypeId = c.Byte(),
                        LeaseTerm = c.Int(),
                        BrokerFeeTypeId = c.Byte(),
                        LengthOfAssignment = c.Int(),
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
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Orders", "DepositTypeId", "dbo.DepositTypes");
            DropForeignKey("dbo.Orders", "BrokerFeeTypeId", "dbo.BrokerFeeTypes");
            DropIndex("dbo.Orders", new[] { "BrokerFeeTypeId" });
            DropIndex("dbo.Orders", new[] { "DepositTypeId" });
            DropColumn("dbo.Orders", "LengthOfAssignment");
            DropColumn("dbo.Orders", "BrokerFeeTypeId");
            DropColumn("dbo.Orders", "LeaseTerm");
            DropColumn("dbo.Orders", "DepositTypeId");
            CreateIndex("dbo.Leases", "CreatedAt", clustered: true);
            CreateIndex("dbo.Leases", "BrokerFeeTypeId");
            CreateIndex("dbo.Leases", "DepositTypeId");
            CreateIndex("dbo.Leases", "Id");
            AddForeignKey("dbo.Leases", "Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Leases", "DepositTypeId", "dbo.DepositTypes", "Id");
            AddForeignKey("dbo.Leases", "BrokerFeeTypeId", "dbo.BrokerFeeTypes", "Id");
        }
    }
}
