namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstimatedArrivalToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "EstimatedArrivalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "EstimatedArrivalDate");
        }
    }
}
