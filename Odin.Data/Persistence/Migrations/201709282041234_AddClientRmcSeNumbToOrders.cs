namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientRmcSeNumbToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SeCustNumb", c => c.String());
            AddColumn("dbo.Orders", "Rmc", c => c.String());
            AddColumn("dbo.Orders", "Client", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Client");
            DropColumn("dbo.Orders", "Rmc");
            DropColumn("dbo.Orders", "SeCustNumb");
        }
    }
}
