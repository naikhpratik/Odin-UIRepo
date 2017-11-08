namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDeleteForHomeFindingPropertiesRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HomeFindingProperties", "Property_Id", "dbo.Properties");
            DropIndex("dbo.HomeFindingProperties", new[] { "Property_Id" });
            AlterColumn("dbo.HomeFindingProperties", "Property_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.HomeFindingProperties", "Property_Id");
            AddForeignKey("dbo.HomeFindingProperties", "Property_Id", "dbo.Properties", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HomeFindingProperties", "Property_Id", "dbo.Properties");
            DropIndex("dbo.HomeFindingProperties", new[] { "Property_Id" });
            AlterColumn("dbo.HomeFindingProperties", "Property_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.HomeFindingProperties", "Property_Id");
            AddForeignKey("dbo.HomeFindingProperties", "Property_Id", "dbo.Properties", "Id");
        }
    }
}
