namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderSchoolNFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SchoolDistrict", c => c.String());
            AddColumn("dbo.Orders", "ClientFileNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ClientFileNumber");
            DropColumn("dbo.Orders", "SchoolDistrict");
        }
    }
}
