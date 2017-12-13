namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLikedToHomeFindingProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeFindingProperties", "Liked", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HomeFindingProperties", "Liked");
        }
    }
}
