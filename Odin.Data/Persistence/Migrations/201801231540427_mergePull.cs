namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergePull : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HomeFindingProperties", "selected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HomeFindingProperties", "selected", c => c.Boolean());
        }
    }
}
