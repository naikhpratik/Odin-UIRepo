namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewingDateToHomeFindingProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeFindingProperties", "ViewingDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HomeFindingProperties", "ViewingDate");
        }
    }
}
