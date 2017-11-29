namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSortOrderAndDefault : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceTypes", "SortOrder", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceTypes", "Default", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceTypes", "Default");
            DropColumn("dbo.ServiceTypes", "SortOrder");
        }
    }
}
