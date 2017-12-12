namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLatLngToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.Orders", "Longitude", c => c.Decimal(precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Longitude");
            DropColumn("dbo.Orders", "Latitude");
        }
    }
}
