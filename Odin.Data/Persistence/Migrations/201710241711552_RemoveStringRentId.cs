namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStringRentId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "RentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "RentId", c => c.Int());
        }
    }
}
