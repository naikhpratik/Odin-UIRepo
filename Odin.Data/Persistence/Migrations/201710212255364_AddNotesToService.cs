namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesToService : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Notes");
        }
    }
}
