namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedByToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "CreatedById", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "CreatedById");
            AddForeignKey("dbo.Notifications", "CreatedById", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "CreatedById" });
            DropColumn("dbo.Notifications", "CreatedById");
        }
    }
}
