namespace Odin.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddUrlLatLngToProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Properties", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.Properties", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.Properties", "SourceUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Properties", "SourceUrl");
            DropColumn("dbo.Properties", "Longitude");
            DropColumn("dbo.Properties", "Latitude");
        }
    }
}
