namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rents", "SquareFootage", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rents", "SquareFootage", c => c.Int(nullable: false));
        }
    }
}
