namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgramNameToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ProgramName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ProgramName");
        }
    }
}
