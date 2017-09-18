namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "TrackingId", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Orders", "TrackingId", unique: true, name: "IX_TrackingID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", "IX_TrackingID");
            AlterColumn("dbo.Orders", "TrackingId", c => c.String());
        }
    }
}
