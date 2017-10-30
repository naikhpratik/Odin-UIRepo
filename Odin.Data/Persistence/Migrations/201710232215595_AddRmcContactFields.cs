namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRmcContactFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "RmcContact", c => c.String());
            AddColumn("dbo.Orders", "RmcContactEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "RmcContactEmail");
            DropColumn("dbo.Orders", "RmcContact");
        }
    }
}
