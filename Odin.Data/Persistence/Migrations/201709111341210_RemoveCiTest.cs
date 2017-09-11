namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCiTest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "CiTest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CiTest", c => c.String());
        }
    }
}
