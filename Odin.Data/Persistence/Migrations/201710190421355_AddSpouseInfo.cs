namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpouseInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SpouseName", c => c.String());
            AddColumn("dbo.Orders", "SpouseVisaType", c => c.String());
            AddColumn("dbo.Orders", "ChildrenEducationPreferences", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ChildrenEducationPreferences");
            DropColumn("dbo.Orders", "SpouseVisaType");
            DropColumn("dbo.Orders", "SpouseName");
        }
    }
}
