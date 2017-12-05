namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustLatLngPrecisionScale : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Properties", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AlterColumn("dbo.Properties", "Longitude", c => c.Decimal(precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Properties", "Longitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Properties", "Latitude", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
