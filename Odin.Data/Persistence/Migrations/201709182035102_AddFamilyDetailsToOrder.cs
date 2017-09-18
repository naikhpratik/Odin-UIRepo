namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFamilyDetailsToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FamilyDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "FamilyDetails");
        }
    }
}
