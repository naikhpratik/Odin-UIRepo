namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingServiceTypeLinking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "ServiceType_Id", "dbo.ServiceTypes");
            DropIndex("dbo.Services", new[] { "ServiceType_Id" });
            RenameColumn(table: "dbo.Services", name: "ServiceType_Id", newName: "ServiceTypeId");
            AlterColumn("dbo.Services", "ServiceTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "ServiceTypeId");
            AddForeignKey("dbo.Services", "ServiceTypeId", "dbo.ServiceTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "ServiceTypeId", "dbo.ServiceTypes");
            DropIndex("dbo.Services", new[] { "ServiceTypeId" });
            AlterColumn("dbo.Services", "ServiceTypeId", c => c.Int());
            RenameColumn(table: "dbo.Services", name: "ServiceTypeId", newName: "ServiceType_Id");
            CreateIndex("dbo.Services", "ServiceType_Id");
            AddForeignKey("dbo.Services", "ServiceType_Id", "dbo.ServiceTypes", "Id");
        }
    }
}
