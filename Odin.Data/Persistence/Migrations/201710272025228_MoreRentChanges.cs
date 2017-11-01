namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreRentChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "HasLaundry", c => c.Boolean());
            AddColumn("dbo.Rents", "NumberOfCarsOwned", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "NumberOfCarsOwned");
            DropColumn("dbo.Rents", "HasLaundry");
        }
    }
}
