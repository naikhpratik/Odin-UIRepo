namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPreTripToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PreTripDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PreTripDate");
        }
    }
}
