namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "IsRemoved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "IsRemoved");
        }
    }
}
