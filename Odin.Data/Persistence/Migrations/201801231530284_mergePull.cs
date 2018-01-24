namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergePull : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeFindingProperties", "selected", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HomeFindingProperties", "selected");
        }
    }
}
