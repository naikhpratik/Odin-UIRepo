namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNameFromService : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Name", c => c.String());
        }
    }
}
