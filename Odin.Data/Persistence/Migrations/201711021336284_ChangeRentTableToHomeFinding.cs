namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRentTableToHomeFinding : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Rents", newName: "HomeFindings");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.HomeFindings", newName: "Rents");
        }
    }
}
