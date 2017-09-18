namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeContactUidToUsersAndPmToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SeContactUid", c => c.Int());
            AddColumn("dbo.Orders", "ProgramManagerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ProgramManagerId");
            AddForeignKey("dbo.Orders", "ProgramManagerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ProgramManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ProgramManagerId" });
            DropColumn("dbo.Orders", "ProgramManagerId");
            DropColumn("dbo.AspNetUsers", "SeContactUid");
        }
    }
}
