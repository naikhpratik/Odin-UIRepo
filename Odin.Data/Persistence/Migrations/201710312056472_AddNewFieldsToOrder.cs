namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DaysAuthorized", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DaysAuthorized");
        }
    }
}
