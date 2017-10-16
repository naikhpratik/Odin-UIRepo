namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRushVipLastContactedToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "LastContactedDate", c => c.DateTime());
            AddColumn("dbo.Orders", "IsRush", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "IsVip", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsVip");
            DropColumn("dbo.Orders", "IsRush");
            DropColumn("dbo.Orders", "LastContactedDate");
        }
    }
}
