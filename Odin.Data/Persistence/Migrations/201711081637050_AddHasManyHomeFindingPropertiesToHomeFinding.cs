namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasManyHomeFindingPropertiesToHomeFinding : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HomeFindingProperties", "HomeFinding_Id", "dbo.HomeFindings");
            DropIndex("dbo.HomeFindingProperties", new[] { "HomeFinding_Id" });
            AlterColumn("dbo.HomeFindingProperties", "HomeFinding_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.HomeFindingProperties", "HomeFinding_Id");
            AddForeignKey("dbo.HomeFindingProperties", "HomeFinding_Id", "dbo.HomeFindings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HomeFindingProperties", "HomeFinding_Id", "dbo.HomeFindings");
            DropIndex("dbo.HomeFindingProperties", new[] { "HomeFinding_Id" });
            AlterColumn("dbo.HomeFindingProperties", "HomeFinding_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.HomeFindingProperties", "HomeFinding_Id");
            AddForeignKey("dbo.HomeFindingProperties", "HomeFinding_Id", "dbo.HomeFindings", "Id");
        }
    }
}
