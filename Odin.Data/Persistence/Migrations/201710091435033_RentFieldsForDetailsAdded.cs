namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentFieldsForDetailsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TempHousingDays", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "TempHousingEndDate", c => c.DateTime());
            AddColumn("dbo.Rents", "NumberOfBedrooms", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "NumberOfBathrooms", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "SquareFootage", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "OwnershipType", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "HousingType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "HousingType");
            DropColumn("dbo.Rents", "OwnershipType");
            DropColumn("dbo.Rents", "SquareFootage");
            DropColumn("dbo.Rents", "NumberOfBathrooms");
            DropColumn("dbo.Rents", "NumberOfBedrooms");
            DropColumn("dbo.Orders", "TempHousingEndDate");
            DropColumn("dbo.Orders", "TempHousingDays");
        }
    }
}
