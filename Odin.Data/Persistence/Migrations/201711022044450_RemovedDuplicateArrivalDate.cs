namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDuplicateArrivalDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "FinalArrivalDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "FinalArrivalDate", c => c.DateTime());
        }
    }
}
