namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedFields : DbMigration
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrokerFeeTypes", t => t.BrokerFeeTypeId)
                .ForeignKey("dbo.DepositTypes", t => t.DepositTypeId)
                .ForeignKey("dbo.Orders", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.DepositTypeId)
                .Index(t => t.BrokerFeeTypeId)
                .Index(t => t.CreatedAt, clustered: true);
            
            CreateTable(
                "dbo.BrokerFeeTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                        SeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepositTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                        SeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AreaTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                        SeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HousingTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                        SeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NumberOfBathroomsTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                        SeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportationTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                        SeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "IsInternational", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "IsAssignment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "EstimatedDepartureDate", c => c.DateTime());
            AddColumn("dbo.Rents", "NumberOfBathroomsTypeId", c => c.Byte());
            AddColumn("dbo.Rents", "HousingTypeId", c => c.Byte());
            AddColumn("dbo.Rents", "TransportationTypeId", c => c.Byte());
            AddColumn("dbo.Rents", "IsFurnished", c => c.Boolean());
            AddColumn("dbo.Rents", "MaxCommute", c => c.Int());
            AddColumn("dbo.Rents", "HasParking", c => c.Boolean());
            AddColumn("dbo.Rents", "HasAC", c => c.Boolean());
            AddColumn("dbo.Rents", "HasExerciseRoom", c => c.Boolean());
            AddColumn("dbo.Rents", "Comments", c => c.String());
            AddColumn("dbo.Rents", "AreaTypeId", c => c.Byte());
            AlterColumn("dbo.Rents", "NumberOfBedrooms", c => c.Int());
            CreateIndex("dbo.Rents", "NumberOfBathroomsTypeId");
            CreateIndex("dbo.Rents", "HousingTypeId");
            CreateIndex("dbo.Rents", "TransportationTypeId");
            CreateIndex("dbo.Rents", "AreaTypeId");
            AddForeignKey("dbo.Rents", "AreaTypeId", "dbo.AreaTypes", "Id");
            AddForeignKey("dbo.Rents", "HousingTypeId", "dbo.HousingTypes", "Id");
            AddForeignKey("dbo.Rents", "NumberOfBathroomsTypeId", "dbo.NumberOfBathroomsTypes", "Id");
            AddForeignKey("dbo.Rents", "TransportationTypeId", "dbo.TransportationTypes", "Id");
            DropColumn("dbo.Rents", "NumberOfBathrooms");
            DropColumn("dbo.Rents", "HousingType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rents", "HousingType", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "NumberOfBathrooms", c => c.Int(nullable: false));
            DropForeignKey("dbo.Rents", "TransportationTypeId", "dbo.TransportationTypes");
            DropForeignKey("dbo.Rents", "NumberOfBathroomsTypeId", "dbo.NumberOfBathroomsTypes");
            DropForeignKey("dbo.Rents", "HousingTypeId", "dbo.HousingTypes");
            DropForeignKey("dbo.Rents", "AreaTypeId", "dbo.AreaTypes");
            DropForeignKey("dbo.Leases", "Id", "dbo.Orders");
            DropForeignKey("dbo.Leases", "DepositTypeId", "dbo.DepositTypes");
            DropForeignKey("dbo.Leases", "BrokerFeeTypeId", "dbo.BrokerFeeTypes");
            DropIndex("dbo.Rents", new[] { "AreaTypeId" });
            DropIndex("dbo.Rents", new[] { "TransportationTypeId" });
            DropIndex("dbo.Rents", new[] { "HousingTypeId" });
            DropIndex("dbo.Rents", new[] { "NumberOfBathroomsTypeId" });
            DropIndex("dbo.Leases", new[] { "CreatedAt" });
            DropIndex("dbo.Leases", new[] { "BrokerFeeTypeId" });
            DropIndex("dbo.Leases", new[] { "DepositTypeId" });
            DropIndex("dbo.Leases", new[] { "Id" });
            AlterColumn("dbo.Rents", "NumberOfBedrooms", c => c.Int(nullable: false));
            DropColumn("dbo.Rents", "AreaTypeId");
            DropColumn("dbo.Rents", "Comments");
            DropColumn("dbo.Rents", "HasExerciseRoom");
            DropColumn("dbo.Rents", "HasAC");
            DropColumn("dbo.Rents", "HasParking");
            DropColumn("dbo.Rents", "MaxCommute");
            DropColumn("dbo.Rents", "IsFurnished");
            DropColumn("dbo.Rents", "TransportationTypeId");
            DropColumn("dbo.Rents", "HousingTypeId");
            DropColumn("dbo.Rents", "NumberOfBathroomsTypeId");
            DropColumn("dbo.Orders", "EstimatedDepartureDate");
            DropColumn("dbo.Orders", "IsAssignment");
            DropColumn("dbo.Orders", "IsInternational");
            DropTable("dbo.TransportationTypes");
            DropTable("dbo.NumberOfBathroomsTypes");
            DropTable("dbo.HousingTypes");
            DropTable("dbo.AreaTypes");
            DropTable("dbo.DepositTypes");
            DropTable("dbo.BrokerFeeTypes");
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
