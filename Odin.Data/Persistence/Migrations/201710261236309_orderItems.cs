namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FinalArrivalDate", c => c.DateTime());
            AddColumn("dbo.Orders", "HomeFindingDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "HomeFindingDate");
            DropColumn("dbo.Orders", "FinalArrivalDate");
        }
    }
}
