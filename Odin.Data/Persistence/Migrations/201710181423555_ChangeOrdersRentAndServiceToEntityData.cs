namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOrdersRentAndServiceToEntityData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Rents", "Id", "dbo.Orders");
            DropIndex("dbo.Rents", new[] { "Id" });
            DropIndex("dbo.Services", new[] { "OrderId" });
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.Rents");
            DropPrimaryKey("dbo.Services");
            DropColumn("dbo.Orders", "Id");
            DropColumn("dbo.Services", "Id");
            DropColumn("dbo.Rents", "Id");
            AddColumn("dbo.Orders", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Version")
                    },
                }));
            AddColumn("dbo.Orders", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                    },
                }));
            AddColumn("dbo.Orders", "UpdatedAt", c => c.DateTimeOffset(precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                    },
                }));
            AddColumn("dbo.Orders", "Deleted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Deleted")
                    },
                }));
            AddColumn("dbo.Rents", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Version")
                    },
                }));
            AddColumn("dbo.Rents", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                    },
                }));
            AddColumn("dbo.Rents", "UpdatedAt", c => c.DateTimeOffset(precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                    },
                }));
            AddColumn("dbo.Rents", "Deleted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Deleted")
                    },
                }));
            AddColumn("dbo.Services", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Version")
                    },
                }));
            AddColumn("dbo.Services", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                    },
                }));
            AddColumn("dbo.Services", "UpdatedAt", c => c.DateTimeOffset(precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                    },
                }));
            AddColumn("dbo.Services", "Deleted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Deleted")
                    },
                }));
            AddColumn("dbo.Orders", "Id", c => c.String(nullable: false, maxLength: 128,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Id")
                    },
                }));
            AddColumn("dbo.Rents", "Id", c => c.String(nullable: false, maxLength: 128,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Id")
                    },
                }));
            AddColumn("dbo.Services", "Id", c => c.String(nullable: false, maxLength: 128,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Id")
                    },
                }));
            AlterColumn("dbo.Services", "OrderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "CreatedAt", clustered: true);
            CreateIndex("dbo.Rents", "CreatedAt", clustered: true);
            CreateIndex("dbo.Services", "CreatedAt", clustered: true);
            AddPrimaryKey("dbo.Orders", "Id");
            AddPrimaryKey("dbo.Rents", "Id");
            AddPrimaryKey("dbo.Services", "Id");
            CreateIndex("dbo.Rents", "Id");
            CreateIndex("dbo.Services", "OrderId");
            AddForeignKey("dbo.Services", "OrderId", "dbo.Orders", "Id");
            AddForeignKey("dbo.Rents", "Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "Id", "dbo.Orders");
            DropForeignKey("dbo.Services", "OrderId", "dbo.Orders");
            DropIndex("dbo.Services", new[] { "CreatedAt" });
            DropIndex("dbo.Services", new[] { "OrderId" });
            DropIndex("dbo.Rents", new[] { "CreatedAt" });
            DropIndex("dbo.Rents", new[] { "Id" });
            DropIndex("dbo.Orders", new[] { "CreatedAt" });
            DropPrimaryKey("dbo.Services");
            DropPrimaryKey("dbo.Rents");
            DropPrimaryKey("dbo.Orders");
            DropColumn("dbo.Orders", "Id");
            DropColumn("dbo.Services", "Id");
            DropColumn("dbo.Rents", "Id");

            AlterColumn("dbo.Services", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Services", "Id", c => c.Int(nullable: false, identity: true,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: "Id", newValue: null)
                    },
                }));
            AddColumn("dbo.Rents", "Id", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: "Id", newValue: null)
                    },
                }));
            AddColumn("dbo.Orders", "Id", c => c.Int(nullable: false, identity: true,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: "Id", newValue: null)
                    },
                }));
            DropColumn("dbo.Services", "Deleted",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Deleted" },
                });
            DropColumn("dbo.Services", "UpdatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "UpdatedAt" },
                });
            DropColumn("dbo.Services", "CreatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "CreatedAt" },
                });
            DropColumn("dbo.Services", "Version",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Version" },
                });
            DropColumn("dbo.Rents", "Deleted",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Deleted" },
                });
            DropColumn("dbo.Rents", "UpdatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "UpdatedAt" },
                });
            DropColumn("dbo.Rents", "CreatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "CreatedAt" },
                });
            DropColumn("dbo.Rents", "Version",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Version" },
                });
            DropColumn("dbo.Orders", "Deleted",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Deleted" },
                });
            DropColumn("dbo.Orders", "UpdatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "UpdatedAt" },
                });
            DropColumn("dbo.Orders", "CreatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "CreatedAt" },
                });
            DropColumn("dbo.Orders", "Version",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Version" },
                });
            AddPrimaryKey("dbo.Services", "Id");
            AddPrimaryKey("dbo.Rents", "Id");
            AddPrimaryKey("dbo.Orders", "Id");
            CreateIndex("dbo.Services", "OrderId");
            CreateIndex("dbo.Rents", "Id");
            AddForeignKey("dbo.Rents", "Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Services", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
