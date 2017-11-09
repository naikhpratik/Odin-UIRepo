namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceFlagToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ServiceFlag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ServiceFlag");
        }
    }
}
